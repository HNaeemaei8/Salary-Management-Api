using EmployeeSalary.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Application.Interfaces
{
    public interface ISalaryCalculationService
    {
        Task<decimal> CalculateAsync(SalaryCalculationRequest request);

    }
}
