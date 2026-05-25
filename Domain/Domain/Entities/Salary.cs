using System.ComponentModel.DataAnnotations;

namespace EmployeeSalary.Domain.Entities
{
    public class Salary
    {
        public int Id { get; set; }

        public string EmployeeFirstName { get; set; } = string.Empty;
        public string EmployeeLastName { get; set; } = string.Empty;

        public DateTime SalaryDate { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal AttractionAllowance { get; set; }
        public decimal TransportationAllowance { get; set; }

        public string OvertimeCalculatorMethod { get; set; } = string.Empty;
        public decimal CalculatedOvertimePay { get; set; }

        public decimal TotalSalary { get; set; }
    }
}
