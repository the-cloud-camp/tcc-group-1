using System.Collections.Generic;
using bojpawnapi.DTO;
using bojpawnapi.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using bojpawnapi.Entities;

namespace bojpawnapi.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly PawnDBContext _context;
        private readonly IMapper _mapper;
        public CustomerService(PawnDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(C => C.CustomerId == id);
            if (customer == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<CustomerDTO>(customer);
            }
 
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomersAsync()
        {
            var customerList = await _context.Customers.ToListAsync();
            if (customerList == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<CustomerDTO>>(customerList);
            }
        }

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO pCustomerPayload)
        {
            var customerEntities = _mapper.Map<CustomerEntities>(pCustomerPayload);

            _context.Customers.Add(customerEntities);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return _mapper.Map<CustomerDTO>(customerEntities);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDTO pCustomerPayload)
        {
            var customerEntities = _mapper.Map<CustomerEntities>(pCustomerPayload);

            _context.Entry(customerEntities).State = EntityState.Modified;
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

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customerEntities = await _context.Customers.FindAsync(id);
            if (customerEntities == null)
            {
                return false;
            }
            else
            {
                _context.Customers.Remove(customerEntities);
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
}