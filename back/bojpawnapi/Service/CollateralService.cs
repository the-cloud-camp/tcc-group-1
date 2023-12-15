using System.Collections.Generic;
using bojpawnapi.DTO;
using bojpawnapi.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using bojpawnapi.Entities;
using bojpawnapi.Service.Metric;
using bojpawnapi.Common.OpenTelemetry;

namespace bojpawnapi.Service
{
    public class CollateralTxService : ICollateralTxService
    {
        private readonly string STATUS_PAWN = "PAWN";
        private readonly string STATUS_ROLL = "ROLL_PAWN";   //ต่อดอก
        private readonly string STATUS_REDEEM = "REDEEM";   //ไถ่ถอน
        private readonly string STATUS_DROP = "DROP";       //หลุดจำนำ-อันนี จริงๆไม่จำเป็นดูสถานะ PAWN ที่ EndDate น้อยกว่าวันปัจจุบันแทนได้

        private readonly decimal INTERESTRATE = 3.0M;


        private readonly ILogger<CollateralTxService> _logger;
        private readonly PawnDBContext _context;
        private readonly IMapper _mapper;
        
        private readonly PawnMetrics _PawnMetrics;

        public CollateralTxService(ILogger<CollateralTxService> logger, PawnDBContext context, IMapper mapper, PawnMetrics pPawnMetrics)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _PawnMetrics = pPawnMetrics;
        }

        public async Task<CollateralTxDTO> GetCollateralTxByIdAsync(int id)
        {
            _logger.LogInformation("[Operation-GetCollatetalById] ID {id}", id);
            var collateralTx = await _context.CollateralTxs
                                             .Include(C => C.CollateralDetaills)
                                             .Include(C => C.Customer)
                                             .Include(C => C.Employee)
                                             .FirstOrDefaultAsync(C => C.CollateralId == id);
            if (collateralTx == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<CollateralTxDTO>(collateralTx);
            }
 
        }

        public async Task<IEnumerable<CollateralTxDTO>> GetCollateralTxsAsync()
        {
            _logger.LogInformation("[Operation-GetAllCollatetal]");
            var collateralTxList = await _context.CollateralTxs
                                                 .Include(C => C.CollateralDetaills)
                                                 .Include(C => C.Customer)
                                                 .Include(C => C.Employee)
                                                 .ToListAsync();
            if (collateralTxList == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<CollateralTxDTO>>(collateralTxList);
            }
        }

        /// <summary>
        ///     ใชตอนครั้งแรก
        /// </summary>
        /// <param name="pCollateralPayload"></param>
        /// <returns></returns>
        public async Task<CollateralTxDTO> AddCollateralTxAsync(CollateralTxDTO pCollateralPayload)
        {
            using var activity = ObservabilityRegistration.ActivitySource.StartActivity(nameof(CollateralTxService.AddCollateralTxAsync));

            _logger.LogInformation("[Operation-AddCollatetal] {@CollateralContract}", pCollateralPayload);

            var CollateralTxEntities = _mapper.Map<CollateralTxEntities>(pCollateralPayload);
            
            _context.CollateralTxs.Add(CollateralTxEntities);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return _mapper.Map<CollateralTxDTO>(CollateralTxEntities);
            }
            else
            {
                return null;
            }
        }

        public async Task<CollateralTxDTO> AddPawnCollateralTxAsync(CollateralTxDTO pCollateralPayload)
        {
            using var activity = ObservabilityRegistration.ActivitySource.StartActivity(nameof(CollateralTxService.AddPawnCollateralTxAsync));

            pCollateralPayload.StatusCode = STATUS_PAWN;
            //pCollateralPayload.CreateDate = DateTime.UtcNow;
            pCollateralPayload.CollateralCode = GetCollateralCode();
            pCollateralPayload.Interest = CalcInterestRate(pCollateralPayload.LoanAmt, INTERESTRATE);

            //using var activity = ObservabilityRegistration.ActivitySource.StartActivity("WeatherForecastController.Get");

            //Log
            _logger.LogInformation("[Operation Pawn] {@CollateralContract} on {Created} by {EmployeeId}", pCollateralPayload, DateTime.Now, pCollateralPayload.EmployeeId);

            //Add Metric
            _PawnMetrics.CollateralPawn();
            _PawnMetrics.IncreaseCollateralContracts();
            _PawnMetrics.RecordContractAmt((double)pCollateralPayload.LoanAmt);
            _PawnMetrics.RecordNumberOfCollateral(pCollateralPayload.CollateralDetaills.Count);
            
            return await AddCollateralTxAsync(pCollateralPayload);
        }

        public async Task<CollateralTxDTO> AddRolloverCollateralTxAsync(CollateralTxDTO pCollateralPayload)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int OldRefId = pCollateralPayload.CollateralId;
                    //ปิดสัญญาเดิม
                    pCollateralPayload.StatusCode = STATUS_ROLL;
                    var result = await UpdateCollateralTxAsync(pCollateralPayload);
                    if (result)
                    {
                        //สร้างสัญญาใหม่
                        CollateralTxDTO newPawn = _mapper.Map<CollateralTxDTO>(pCollateralPayload);
                        newPawn = ResetPawnId(newPawn);
                        newPawn.PrevCollateralId = OldRefId;
                        newPawn.CollateralCode = GetCollateralCode();
                        newPawn.StatusCode = STATUS_PAWN;
                        newPawn.Interest = CalcInterestRate(pCollateralPayload.LoanAmt, INTERESTRATE);

                        CollateralTxDTO newPawnResult = await AddCollateralTxAsync(newPawn);
                        transaction.Commit();
                        
                        //Log
                        _logger.LogInformation("[Operation-Rollover] {@CollateralContract} on {Created} by {EmployeeId}", newPawn, DateTime.Now, newPawn.EmployeeId);

                        //Add Metric
                        _PawnMetrics.CollateralRollOver();

                        _PawnMetrics.RecordContractAmt((double)newPawnResult.LoanAmt);
                        _PawnMetrics.RecordNumberOfCollateral(newPawnResult.CollateralDetaills.Count);

                        return newPawnResult;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    //Someone help me
                    throw ex;
                }
            }
        }

        public CollateralTxDTO ResetPawnId(CollateralTxDTO pSource)
        {
            pSource.CollateralId = 0;
            foreach (CollateralTxDetailDTO item in pSource.CollateralDetaills)
            {
                item.CollateralDetailId = 0;
                item.CollateralId = 0;
            }
            return pSource;
        }

        public async Task<bool> AddRedeemCollateralTxAsync(CollateralTxDTO pCollateralPayload)
        {
            //ปิดสัญญาเดิม
            pCollateralPayload.StatusCode = STATUS_REDEEM;
        
            //Log
            _logger.LogInformation("[Operation-Redeem] {@CollateralContract} on {Created} by {EmployeeId}", pCollateralPayload, DateTime.Now, pCollateralPayload.EmployeeId);

            //Add Metric
            _PawnMetrics.CollateralRedeem();
            _PawnMetrics.DecreaseCollateralContracts();

            return await UpdateCollateralTxAsync(pCollateralPayload);
        }

        public string GetCollateralCode()
        {
            return "COLL" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }

        public decimal CalcInterestRate(decimal pLoanAmt, decimal pInterestRate)
        {
            return pLoanAmt * (pInterestRate / 100);
        }

        public async Task<bool> UpdateCollateralTxAsync(CollateralTxDTO pCollateralPayload)
        {
            _logger.LogInformation("[Operation-EditCollatetal] {@CollateralContract}", pCollateralPayload);

            var CollateralTxEntities = _mapper.Map<CollateralTxEntities>(pCollateralPayload);

            _context.Entry(CollateralTxEntities).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async  Task<bool> DeleteCollateralTxAsync(int id)
        {
            _logger.LogInformation("[Operation-DeleteCollatetal] {id}", id);

            var collateralTx = await _context.CollateralTxs.FindAsync(id);
            if (collateralTx == null)
            {
                return false;
            }

            _context.CollateralTxs.Remove(collateralTx);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}