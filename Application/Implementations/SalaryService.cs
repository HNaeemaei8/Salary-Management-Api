using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;


namespace EmployeeSalary.Application.Implementations
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IDataParserFactory _parserFactory;


        private readonly decimal _hourlyRate;

        public SalaryService(ISalaryRepository salaryRepository,IDataParserFactory parserFactory,decimal hourlyRate = 10000) // مقدار پیش‌فرض 10000 تومان
        {
            _salaryRepository = salaryRepository;
            _parserFactory = parserFactory;
            _hourlyRate = hourlyRate;
        }

        public async Task<Salary> AddSalaryAsync(SalaryCalculationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeFirstName) || string.IsNullOrWhiteSpace(request.EmployeeLastName))
            {
                throw new ArgumentException("نام و نام خانوادگی کارمند الزامی است.");
            }

            decimal overtimePay = 0;
            if (request.OvertimeHours > 0)
            {
                overtimePay = request.OvertimeHours * _hourlyRate;
            }

            decimal tax = request.Tax;

            decimal totalSalary = request.BaseSalary +
                                  request.AttractionAllowance +
                                  request.TransportationAllowance +
                                  overtimePay -
                                  tax;

            var salary = new Salary
            {
                EmployeeFirstName = request.EmployeeFirstName,
                EmployeeLastName = request.EmployeeLastName,
                SalaryDate = request.SalaryDate,
                BaseSalary = request.BaseSalary,
                AttractionAllowance = request.AttractionAllowance,
                TransportationAllowance = request.TransportationAllowance,
                OvertimeCalculatorMethod = request.OverTimeCalculatorMethod,
                CalculatedOvertimePay = overtimePay,
                TotalSalary = totalSalary
            };

            await _salaryRepository.AddAsync(salary);
            return salary;
        }

        public async Task<Salary?> UpdateSalaryAsync(int id, SalaryUpdateData data)
        {
            var salary = await _salaryRepository.GetByIdAsync(id);
            if (salary == null) return null;
            if (data.BasicSalary.HasValue) salary.BaseSalary = data.BasicSalary.Value;
            if (data.Allowance.HasValue) salary.AttractionAllowance = data.Allowance.Value;
            if (data.Transport.HasValue) salary.TransportationAllowance = data.Transport.Value;

            if (data.OvertimePay.HasValue) salary.CalculatedOvertimePay = data.OvertimePay.Value;


            if (data.PaymentDate.HasValue) salary.SalaryDate = data.PaymentDate.Value;

            salary.TotalSalary = salary.BaseSalary + salary.AttractionAllowance + salary.TransportationAllowance + salary.CalculatedOvertimePay;

            await _salaryRepository.UpdateAsync(salary);
            return salary;
        }

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            await _salaryRepository.DeleteAsync(id);
            return true;
        }

        public async Task<Salary?> GetSalaryByIdAsync(int id)
        {
            return await _salaryRepository.GetByIdAsync(id);
        }

        public async Task<List<Salary>> GetSalariesForEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var startYear = startDate.Year;
            var startMonth = startDate.Month;
            var endYear = endDate.Year;
            var endMonth = endDate.Month;

            return await _salaryRepository.GetRangeAsync(employeeId, startYear, startMonth, endYear, endMonth);
        }

        public async Task<List<Salary>> ProcessRawDataAsync(string format, string rawData)
        {
            var requests = await _parserFactory.GetParser(format).ParseAsync(rawData);

            var results = new List<Salary>();

            foreach (var request in requests)
            {
                var salary = await AddSalaryAsync(request);
                results.Add(salary);
            }

            return results; 
        }
    }
}
   

