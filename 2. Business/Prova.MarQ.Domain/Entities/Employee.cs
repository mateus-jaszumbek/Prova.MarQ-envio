using System.ComponentModel.DataAnnotations;

namespace Prova.MarQ.Domain.Entities
{
    public class Employee : Base
    {
        public int EmployeeId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Document { get; set; } = string.Empty;
        [MaxLength(4)]
        public string PIN { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Company Company { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; }
    }
}
