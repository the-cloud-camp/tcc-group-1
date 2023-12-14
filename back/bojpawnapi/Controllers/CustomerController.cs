using Microsoft.AspNetCore.Mvc;
using bojpawnapi.Service;
using bojpawnapi.DTO;
using Microsoft.AspNetCore.Authorization;

namespace bojpawnapi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;

        }
  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customerList = await _customerService.GetCustomersAsync();
            if (customerList != null)
            {
                var response = new APIResponseDTO<IEnumerable<CustomerDTO>>
                {
                    Code = "S201-001-01",
                    Message = "Get Customers successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = customerList
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<IEnumerable<CustomerDTO>>
                {
                    Code = "S204-001-01",
                    Message = "Get Customers But No Content",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                //return NoContent(response);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, response); 

            }
        }
        //GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            var response = new APIResponseDTO<CustomerDTO>();
            if (customer == null)
            {
                //return NotFound();
                response = new APIResponseDTO<CustomerDTO>
                {
                    Code = "E404-001-02",
                    Message = "Get Customer " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 

            }

            response = new APIResponseDTO<CustomerDTO>
            {
                Code = "S200-001-02",
                Message = "Get Customer: " + id,
                Description = "Request successful",
                Timestamp = DateTime.UtcNow,
                Data = customer
            };
            return Ok(response);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customer)
        {
            var result = await _customerService.AddCustomerAsync(customer);
            if (result != null)
            {
                //return CreatedAtAction(nameof(GetCustomer), new { id = result.CustomerId }, result);

                var response = new APIResponseDTO<CustomerDTO>
                {
                    Code = "S201-001-03",
                    Message = "Customers created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                //return BadRequest();
                var response = new APIResponseDTO<CustomerDTO>
                {
                    Code = "E400-001-03",
                    Message = "Insert Customer But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO customer)
        {
            if (id != customer.CustomerId)
            {
                var response = new APIResponseDTO<CustomerDTO>
                {
                    Code = "E400-001-04",
                    Message = "Update Customer But id mismatch",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }

            var result = await _customerService.UpdateCustomerAsync(customer);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S201-001-04",
                    Message = "Update Customer " + id + " successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };
                return Ok(response);
            }
            else
            {
                //return NotFound();
                var response = new APIResponseDTO<bool>
                {
                    Code = "E404-001-05",
                    Message = "Update Customer But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S200-001-05",
                    Message = "Delete Customer " + id + " successful",
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
                    Code = "E404-001-05",
                    Message = "Delete Customer " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }
    }

}