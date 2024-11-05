using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using Prova.MarQ.Infra.Interfaces;

namespace Prova.MarQ.Infra.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly ProvaMarqDbContext _context;
        private readonly ILogger<EmployeeServices> _logger;
        public EmployeeServices(ProvaMarqDbContext context, ILogger<EmployeeServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Employee> AddEmployee(Employee employeeName)
        {
            try
            {
                var existingEmployee = await _context.Tbemployees
                    .FirstOrDefaultAsync(e => e.Document == employeeName.Document);

                if (existingEmployee != null)
                {
                    throw new Exception("Um funcionaário com o mesmo documento já existe.");
                }

                employeeName.PIN = await GerateUniquePin();

                _context.Tbemployees.Add(employeeName);
                await _context.SaveChangesAsync();

                return employeeName;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Erro ao adicionar o funcionário: {dbEx.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o funcionário ao banco de dados.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o funcionário.");
            }
        }

        public async Task<string> GerateUniquePin(int? employeeId = null)
        {
            string pin = "";
            bool isUnique;
            do
            {
                pin = new Random().Next(1000, 9999).ToString();

                isUnique = !await _context.Tbemployees.AnyAsync(e => e.PIN == pin);
            }
            while (!isUnique);

            return pin;
        }

        async Task<Employee?> IEmployeeServices.UpdateEmployee(int employeeId, Employee employeeName)
        {
            var entry = await _context.Tbemployees.FindAsync(employeeId);
            if (entry == null) { return null; }

            if (entry.Document != employeeName.Document)
            {
                var existingEmployee = await _context.Tbemployees
                                .FirstOrDefaultAsync(e => e.Document == employeeName.Document);

                if (existingEmployee != null)
                {
                    throw new Exception("Um funcionaário com o mesmo documento já existe.");
                }
            }

            entry.Name = employeeName.Name;
            entry.Document = employeeName.Document;
            entry.PIN = employeeName.PIN;
            entry.IsDeleted = false;

            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            try
            {
                var entry = await _context.Tbemployees.FindAsync(employeeId);
                if (entry == null) { return false; }

                entry.IsDeleted = true;
                _context.Tbemployees.Update(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Erro ao excluir o funcionário: {dbEx.Message}");
                throw new Exception("Ocorreu um erro ao tentar excluir o funcionário do banco de dados.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar excluir o funcionário.");
            }
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _context.Tbemployees
                .Include(e => e.TimeEntries)
                .Where(e => e.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                return await _context.Tbemployees
                 .Include(e => e.TimeEntries)
                 .Where(e => e.IsDeleted == false)
                 .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar procurar os funcionários.");
            }
        }

        public async Task<Employee?> GetEmployeeByNameAsync(string employeeName)
        {
            try
            {
                return await _context.Tbemployees
               .Include(employeeName => employeeName.TimeEntries)
               .Where(c => c.Name.ToLower().Contains(employeeName.ToLower()) || c.Document.Contains(employeeName))
               .FirstOrDefaultAsync(e => e.Name == employeeName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar procurar o funcionário.");
            }


        }
    }
}
