using System.Collections.Generic;
using bojpawnapi.DTO;
namespace bojpawnapi.Service
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int employeeId);
        Task<CustomerDTO> AddCustomerAsync(CustomerDTO employee);
        Task<bool> UpdateCustomerAsync(CustomerDTO employee);
        Task<bool> DeleteCustomerAsync(int employeeId);
    }
}