using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bojpawnapi.Service;
using bojpawnapi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bojpawnapi.Service.Auth;
using bojpawnapi.DTO.Auth;
using bojpawnapi.Common.Auth;

namespace bojpawnapi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        
        private readonly IAuthService _authService;

        public EmployeeController(IEmployeeService pEmployeeService, IAuthService pAuthService)
        {
            _employeeService = pEmployeeService;
            _authService = pAuthService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employeeList = await _employeeService.GetEmployeesAsync();
            if (employeeList != null)
            {
                var response = new APIResponseDTO<IEnumerable<EmployeeDTO>>
                {
                    Code = "S201-002-01",
                    Message = "Get Employees successful",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = employeeList
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<IEnumerable<EmployeeDTO>>
                {
                    Code = "S204-002-01",
                    Message = "Get Employee But No Content",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, response); 

            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            var response = new APIResponseDTO<EmployeeDTO>();
            if (employee == null)
            {
                response = new APIResponseDTO<EmployeeDTO>
                {
                    Code = "E404-002-02",
                    Message = "Get Employee " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 

            }

            response = new APIResponseDTO<EmployeeDTO>
            {
                Code = "S200-002-02",
                Message = "Get Employee: " + id,
                Description = "Request successful",
                Timestamp = DateTime.UtcNow,
                Data = employee
            };
            return Ok(response);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employee)
        {
            var (status, message) = await _authService.Registration(new RegistrationDTO
            {
                Username = employee.Username,
                Password = employee.Password,
                Email = employee.Email
            }, UserRoles.Employee);

            if (status == 0)
            {
                var response = new APIResponseDTO<EmployeeDTO>
                {
                    Code = "E400-002-03",
                    Message = message,
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }

            var result = await _employeeService.AddEmployeeAsync(employee);
            if (result != null)
            {
                var response = new APIResponseDTO<EmployeeDTO>
                {
                    Code = "S201-002-03",
                    Message = "Employee created successfully",
                    Description = "The item was added to the database",
                    Timestamp = DateTime.UtcNow,
                    Data = result
                };

                return Ok(response);
            }
            else
            {
                var response = new APIResponseDTO<EmployeeDTO>
                {
                    Code = "E400-002-03",
                    Message = "Insert Employee But Bad Request",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employee)
        {
            if (id != employee.EmployeeId)
            {
                var response = new APIResponseDTO<CustomerDTO>
                {
                    Code = "E400-002-04",
                    Message = "Update Employee But id mismatch",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, response);
            }

            var result = await _employeeService.UpdateEmployeeAsync(employee);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S201-002-04",
                    Message = "Update Employee " + id + " successful",
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
                    Code = "E404-002-05",
                    Message = "Update Employee But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (result)
            {
                var response = new APIResponseDTO<bool>
                {
                    Code = "S200-002-05",
                    Message = "Delete Employee " + id + " successful",
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
                    Code = "E404-002-05",
                    Message = "Delete Employee " + id + " But Not Found",
                    Description = "Request successful",
                    Timestamp = DateTime.UtcNow,
                    Data = false
                };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, response); 
            }
        }
    }
}