using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;

namespace Prova.MarQ.Infra
{
    public class ProvaMarqDbContext : DbContext
    {
        public ProvaMarqDbContext(DbContextOptions<ProvaMarqDbContext> options) : base(options) { }

        public DbSet<Employee> Tbemployees { get; set; }
        public DbSet<Company> Tbcompanies { get; set; }
        public DbSet<TimeEntry> TbtimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.CompanyId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Document).IsRequired().HasMaxLength(20);
                entity.Property(c => c.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Document).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.Employees)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(t => t.TimeEntryId);
                entity.Property(t => t.StartTime).IsRequired();
                entity.Property(t => t.IsDeleted).HasDefaultValue(false);

                entity.HasOne(t => t.Employee)
                   .WithMany(e => e.TimeEntries)
                   .HasForeignKey(t => t.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
