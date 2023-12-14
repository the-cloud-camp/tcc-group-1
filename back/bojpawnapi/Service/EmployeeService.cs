using System.Collections.Generic;
using bojpawnapi.DTO;
using bojpawnapi.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using bojpawnapi.Entities;
using bojpawnapi.Service.Auth;
using bojpawnapi.DTO.Auth;
using bojpawnapi.Common.Auth;

namespace bojpawnapi.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly PawnDBContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public EmployeeService(PawnDBContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                                         .FirstOrDefaultAsync(C => C.EmployeeId == id);
            if (employee == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<EmployeeDTO>(employee);
            }
 
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync()
        {
            var employeeList = await _context.Employees.ToListAsync();
            if (employeeList == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<EmployeeDTO>>(employeeList);
            }
        }

        public async Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO pEmployeePayload)
        {
            var employeeEntities = _mapper.Map<EmployeeEntities>(pEmployeePayload);

            _context.Employees.Add(employeeEntities);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return _mapper.Map<EmployeeDTO>(employeeEntities);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO pEmployeePayload)
        {
            var employeeEntities = _mapper.Map<EmployeeEntities>(pEmployeePayload);

            _context.Entry(employeeEntities).State = EntityState.Modified;
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

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return false;
            }
            else
            {
                _context.Employees.Remove(employee);
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