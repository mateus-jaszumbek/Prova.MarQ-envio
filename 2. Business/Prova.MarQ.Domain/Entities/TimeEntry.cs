namespace Prova.MarQ.Domain.Entities
{
    public class TimeEntry : Base
    {
        public int TimeEntryId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public bool IsEntry { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

    }
}

