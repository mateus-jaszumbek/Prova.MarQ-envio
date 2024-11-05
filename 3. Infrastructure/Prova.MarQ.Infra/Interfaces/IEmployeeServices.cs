using Microsoft.Extensions.Logging;
using Prova.MarQ.Domain.Entities;

namespace Prova.MarQ.Infra.Interfaces
{
    public interface IEmployeeServices
    {
        Task<Employee> AddEmployee(Employee employeeName);
        Task<Employee?> UpdateEmployee(int employeeId, Employee employeeName);
        Task<bool> DeleteEmployee(int employeeId);

        Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee?> GetEmployeeByNameAsync(string employeeName);

    }
}
