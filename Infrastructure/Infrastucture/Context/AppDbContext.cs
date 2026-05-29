using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Salary> Salaries => Set<Salary>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.Property(e => e.EmployeeFirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EmployeeLastName).IsRequired().HasMaxLength(100);

            });
        }
    }
}