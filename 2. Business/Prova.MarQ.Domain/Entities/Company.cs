using System.ComponentModel.DataAnnotations;

namespace Prova.MarQ.Domain.Entities
{
    public class Company : Base
    {
        public int CompanyId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Document { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public ICollection<Employee> Employees { get; set; }

    }
}
