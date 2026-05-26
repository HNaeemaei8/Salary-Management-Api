
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalaryService
    {
        Task<Salary> AddSalaryAsync(SalaryCalculationRequest request);
        Task<Salary?> UpdateSalaryAsync(int id, SalaryUpdateData data);
        Task<bool> DeleteSalaryAsync(int id);
        Task<Salary?> GetSalaryByIdAsync(int id);
        Task<List<Salary>> GetSalariesForEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<List<Salary>> ProcessRawDataAsync(string format, string rawData);
    }
}


