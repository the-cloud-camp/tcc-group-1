using System.Collections.Generic;
using bojpawnapi.DTO;
namespace bojpawnapi.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int customerId);
        Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO customer);
        Task<bool> UpdateEmployeeAsync(EmployeeDTO customer);
        Task<bool> DeleteEmployeeAsync(int customerId);
    }
}