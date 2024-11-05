using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using Prova.MarQ.Infra.Interfaces;

namespace Prova.MarQ.Infra.Services
{
    public class TimeEntryServices : ITimeEntryServices
    {
        private readonly ProvaMarqDbContext _context;
        public TimeEntryServices(ProvaMarqDbContext context)
        {
            _context = context;
        }

        public async Task<TimeEntry> AddTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                _context.TbtimeEntries.Add(timeEntry);
                await _context.SaveChangesAsync();
                return timeEntry;
            }
            catch (DbUpdateException dbEx)
            {

                Console.WriteLine($"Erro ao adicionar o Ponto: {dbEx.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o ponto ao banco de dados.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o ponto.");
            }


        }
        public async Task<TimeEntry?> UpdateTimeEntry(int timeEntryId, TimeEntry timeEntry)
        {
            try
            {
                var entry = await _context.TbtimeEntries.FindAsync(timeEntryId);
                if (entry == null) { return null; }

                entry.StartTime = timeEntry.StartTime;
                entry.EndTime = timeEntry.EndTime;
                entry.IsEntry = timeEntry.IsEntry;
                entry.IsDeleted = false;

                await _context.SaveChangesAsync();
                return entry;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Erro ao atualizar o ponto: {dbEx.Message}");
                throw new Exception("Ocorreu um erro ao tentar atualizar o ponto no banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar atualizar o ponto.");
            }
        }

        public async Task<bool> DeleteTimeEntry(int timeEntryId)
        {
            try
            {
                var entry = await _context.TbtimeEntries
                    .FirstOrDefaultAsync(e => e.TimeEntryId == timeEntryId && !e.IsDeleted);

                if (entry == null) { return false; }

                entry.IsDeleted = true;
                _context.TbtimeEntries.Update(entry);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Erro ao atualizar a empresa: {dbEx.Message}");
                throw new Exception("Ocorreu um erro ao tentar atualizar o ponto no banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar atualizar o ponto.");
            }
        }

        public async Task<List<TimeEntry>> GetAllTimeEntrysAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _context.TbtimeEntries
                    .Where(t => t.IsDeleted == false)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar encontrar os pontos.");
            }
        }

        public async Task<TimeEntry?> GetTimeEntryByIdAsync(int timeEntryId)
        {
            try
            {
                return await _context.TbtimeEntries
               .Where(t => t.IsDeleted == false)
               .FirstOrDefaultAsync(e => e.TimeEntryId == timeEntryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar Procurar o ponto.");
            }

        }

        public async Task<TimeEntry> RegisterTimeEntryByPinAsync(string pin)
        {
            try
            {
                var employee = await _context.Tbemployees.FirstOrDefaultAsync(e => e.PIN == pin);
                if (employee == null)
                {
                    throw new Exception("Funcionário não encontrado com esse PIN.");
                }

                var timeEntry = new TimeEntry
                {
                    EmployeeId = employee.EmployeeId,
                    StartTime = DateTime.UtcNow,
                    IsEntry = true,
                    IsDeleted = false
                };

                await AddTimeEntry(timeEntry);

                return timeEntry;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Erro ao adicionar o ponto: {dbEx.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o ponto ao banco de dados.");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao tentar adicionar o ponto.");
            }
        }
    }
}
