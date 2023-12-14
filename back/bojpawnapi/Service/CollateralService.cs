using System.Collections.Generic;
using bojpawnapi.DTO;
using bojpawnapi.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using bojpawnapi.Entities;

namespace bojpawnapi.Service
{
    public class CollateralTxService : ICollateralTxService
    {
        private readonly string STATUS_PAWN = "PAWN";
        private readonly string STATUS_ROLL = "ROLL_PAWN";   //ต่อดอก
        private readonly string STATUS_REDEEM = "REDEEM";   //ไถ่ถอน
        private readonly string STATUS_DROP = "DROP";       //หลุดจำนำ-อันนี จริงๆไม่จำเป็นดูสถานะ PAWN ที่ EndDate น้อยกว่าวันปัจจุบันแทนได้

        private readonly decimal INTERESTRATE = 3.0M;

        private readonly PawnDBContext _context;
        private readonly IMapper _mapper;
        public CollateralTxService(PawnDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CollateralTxDTO> GetCollateralTxByIdAsync(int id)
        {
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
            pCollateralPayload.StatusCode = STATUS_PAWN;
            //pCollateralPayload.CreateDate = DateTime.UtcNow;
            pCollateralPayload.CollateralCode = GetCollateralCode();
            pCollateralPayload.Interest = CalcInterestRate(pCollateralPayload.LoanAmt, INTERESTRATE);
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