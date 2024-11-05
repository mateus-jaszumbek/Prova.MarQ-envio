using Prova.MarQ.Domain.Entities;

namespace Prova.MarQ.Infra.Interfaces
{
    public interface ITimeEntryServices
    {
        Task<TimeEntry> AddTimeEntry(TimeEntry timeEntry);
        Task<TimeEntry?> UpdateTimeEntry(int timeEntryId, TimeEntry timeEntry);
        Task<bool> DeleteTimeEntry(int timeEntryId);

        Task<List<TimeEntry>> GetAllTimeEntrysAsync(int pageNumber, int pageSize);
        Task<TimeEntry?> GetTimeEntryByIdAsync(int timeEntryId);
        Task<TimeEntry> RegisterTimeEntryByPinAsync(string pin);
    }
}
