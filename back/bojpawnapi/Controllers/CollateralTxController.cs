using Microsoft.AspNetCore.Mvc;
using bojpawnapi.Service;
using bojpawnapi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace bojpawnapi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CollateralsTxController : ControllerBase
    {
        private readonly ICollateralTxService _collateralTxService;

        public CollateralsTxController(ICollateralTxService collateralTxService)
        {
            _collateralTxService = collateralTxService;
        }

        // GET: api/Collaterals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollateralTxDTO>>> GetCollateralTxs()
        {
            var collateralList = await _collateralTxService.GetCollateralTxsAsync();
            if (collateralList != null)
            {
                var response = new APIResponseDTO<IEnumerable<CollateralTxDTO>>
                {
                    Code = "S201-003-01",
                    Message = "Get Collateral successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = collateralList
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<IEnumerable<CollateralTxDTO>>
                {
                    Code = "S204-003-01",
                    Message = "Get Collateral But No Content",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, response); 
            }
        }

        // GET: api/Collaterals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollateralTxDTO>> GetCollateralTx(int id)
        {
            var collateralTx = await _collateralTxService.GetCollateralTxByIdAsync(id);
            var response = new APIResponseDTO<CollateralTxDTO>();
            if (collateralTx == null)
            {
                //return NotFound();
                response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "E404-003-02",
                    Message = "Get Collateral " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 

            }

            response = new APIResponseDTO<CollateralTxDTO>
            {
                Code = "S200-003-02",
                Message = "Get Collateral: " + id,
                Description = "Request successful",
                Timestamp = DateTime.UtcNow,
                Data = collateralTx
            };
            return Ok(response);
        }

        // POST: api/Collaterals
        [HttpPost]
        public async Task<ActionResult<CollateralTxDTO>> PostCollateralTx(CollateralTxDTO collateralTx)
        {
            var result = await _collateralTxService.AddCollateralTxAsync(collateralTx);
            if (result != null)
            {
                //return CreatedAtAction("GetCollateralTx", new { id = result.CollateralId }, result);
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "S201-003-03",
                    Message = "Collateral created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "E400-003-03",
                    Message = "Insert Collateral But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }

        // PUT: api/Collaterals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollateralTx(int id, CollateralTxDTO collateralTx)
        {
            if (id != collateralTx.CollateralId)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "E400-003-04",
                    Message = "Update Collateral But id mismatch",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }

            var result = await _collateralTxService.UpdateCollateralTxAsync(collateralTx);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S201-003-04",
                    Message = "Update Collateral " + id + " successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "E404-003-05",
                    Message = "Update Collateral But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }

        

        // DELETE: api/Collaterals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCollateralTx(int id)
        {
            var result = await _collateralTxService.DeleteCollateralTxAsync(id);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S200-003-05",
                    Message = "Delete Collateral " + id + " successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = true
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "E404-003-05",
                    Message = "Delete Collateral " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }

        //==============================
        //Business
        //==============================
        // POST: api/pawn
        [HttpPost("pawn")]
        public async Task<ActionResult<CollateralTxDTO>> PostPawnCollateralTx(CollateralTxDTO collateralTx)
        {
            var result = await _collateralTxService.AddPawnCollateralTxAsync(collateralTx);
            if (result != null)
            {
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "S201-003-10",
                    Message = "Pawn created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "E400-003-10",
                    Message = "Insert Collateral But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }

        // POST: api/rollover
        [HttpPost("rollover")]
        public async Task<ActionResult<CollateralTxDTO>> PostRolloverCollateralTx(CollateralTxDTO collateralTx)
        {
            var result = await _collateralTxService.AddRolloverCollateralTxAsync(collateralTx);
            if (result != null)
            {
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "S201-003-11",
                    Message = "Rollover created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<CollateralTxDTO>
                {
                    Code = "E400-003-11",
                    Message = "Insert Collateral But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }

        // POST: api/redeem
        [HttpPost("redeem")]
        public async Task<ActionResult<bool>> PostRedeemCollateralTx(CollateralTxDTO collateralTx)
        {
            var result = await _collateralTxService.AddRedeemCollateralTxAsync(collateralTx);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S201-003-12",
                    Message = "Redeem created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "E400-003-12",
                    Message = "Insert Collateral But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }
        
    }
}