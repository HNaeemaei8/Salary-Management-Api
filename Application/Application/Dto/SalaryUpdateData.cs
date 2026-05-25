using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Application.Dto
{
    public class SalaryUpdateData
    {
        public decimal? BasicSalary { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Transport { get; set; } 
        public decimal? OvertimePay { get; set; }
        public decimal? Bonuses { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? Tax { get; set; } 
        public string? Notes { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? SalaryYear { get; set; }
        public int? SalaryMonth { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
    }
}
