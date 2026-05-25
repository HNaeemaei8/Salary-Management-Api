using EmployeeSalary.Domain.Entities;

namespace EmployeeSalary.Domain.Interfaces
{
        public interface ISalaryRepository
        {
        Task AddAsync(Salary salary);
        Task UpdateAsync(Salary record);
        Task DeleteAsync(int id);
        Task<Salary?> GetByIdAsync(int id);
        Task<List<Salary>> GetRangeAsync(int employeeId, int startYear, int startMonth, int endYear, int endMonth);
    }

}

