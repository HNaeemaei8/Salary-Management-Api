namespace EmployeeSalary.Application.Dto
{
        public class SalaryCalculationRequest
        {
            public string EmployeeFirstName { get; set; } = string.Empty;
            public string EmployeeLastName { get; set; } = string.Empty;
            public DateTime SalaryDate { get; set; }

            public decimal BaseSalary { get; set; }
            public decimal AttractionAllowance { get; set; }
            public decimal TransportationAllowance { get; set; }
            public decimal OvertimePay { get; set; }
            public decimal Tax { get; set; }

            public string OverTimeCalculatorMethod { get; set; } = string.Empty;
            public int OvertimeHours { get; set; }

            public decimal HoursWorked { get; set; }    
            public decimal NormalHours { get; set; } = 176;  

    }
}



