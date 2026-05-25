using Dapper;
using EmployeeSalary.Domain.Entities;
using EmployeeSalary.Domain.Interfaces;
using EmployeeSalary.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeSalary.Infrastucture.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public SalaryRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
      
        public async Task AddAsync(Salary salary)
        {
            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Salary salary)
        {
            _context.Salaries.Update(salary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary != null)
            {
                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Salary?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Salaries WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Salary>(sql, new { Id = id });
        }

        public async Task<List<Salary>> GetRangeAsync(int employeeId, int startYear, int startMonth, int endYear, int endMonth)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT * FROM Salaries 
                    WHERE EmployeeId = @EmployeeId 
                    AND (SalaryYear > @StartYear OR (SalaryYear = @StartYear AND SalaryMonth >= @StartMonth))
                    AND (SalaryYear < @EndYear OR (SalaryYear = @EndYear AND SalaryMonth <= @EndMonth))";

            var parameters = new
            {
                EmployeeId = employeeId,
                StartYear = startYear,
                StartMonth = startMonth,
                EndYear = endYear,
                EndMonth = endMonth
            };

            return (await connection.QueryAsync<Salary>(sql, parameters)).ToList();
        }
    }
}
   

   

