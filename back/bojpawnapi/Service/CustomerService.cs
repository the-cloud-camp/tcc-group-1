using System.Collections.Generic;
using bojpawnapi.DTO;
using bojpawnapi.DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using bojpawnapi.Entities;
using bojpawnapi.Service.Metric;

namespace bojpawnapi.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly PawnDBContext _context;
        private readonly IMapper _mapper;
        
        private readonly PawnMetrics _PawnMetrics;


        public CustomerService(ILogger<CustomerService> logger, PawnDBContext context, IMapper mapper, PawnMetrics pawnMetrics)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _PawnMetrics = pawnMetrics;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            _logger.LogInformation("[Operation-GetCustomerById] ID {id}", id);

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
            _logger.LogInformation("[Operation-AddCustomer] {@CustomerPayload}", pCustomerPayload);

            var customerEntities = _mapper.Map<CustomerEntities>(pCustomerPayload);

            _context.Customers.Add(customerEntities);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                //Metric
                _PawnMetrics.IncreaseCustomer();

                return _mapper.Map<CustomerDTO>(customerEntities);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDTO pCustomerPayload)
        {
            _logger.LogInformation("[Operation-UpdateCustomer] {@CustomerPayload}", pCustomerPayload);

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
            _logger.LogInformation("[Operation-DeleteCustomer] {id}", id);

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
                    //Metric
                    _PawnMetrics.DecreaseCustomer();
                    
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