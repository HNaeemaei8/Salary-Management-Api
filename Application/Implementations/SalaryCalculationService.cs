
using Application.Application.Interfaces;
using EmployeeSalary.Application.Dto;
using Microsoft.Extensions.Logging;
using OvetimePolicies;
using System.Reflection;

public class SalaryCalculationService : ISalaryCalculationService
{
    private readonly ILogger<SalaryCalculationService> _logger;

    public SalaryCalculationService(ILogger<SalaryCalculationService> logger)
    {
        _logger = logger;
    }


    public async Task<decimal> CalculateAsync(SalaryCalculationRequest request)
        {
            decimal baseSalary = request.BaseSalary;

            _logger.LogInformation($"Calculating overtime for employee {request.EmployeeFirstName} {request.EmployeeLastName}");

            decimal overtimePay = 0;
            try
            {
                Type calculatorType = typeof(OvertimeCalculator);

                MethodInfo methodInfo = calculatorType.GetMethod(
                    request.OverTimeCalculatorMethod,
                    new Type[] { typeof(decimal), typeof(int), typeof(int) });

                if (methodInfo != null)
                {
                    overtimePay = (decimal)methodInfo.Invoke(null, new object[]
                    {
                    baseSalary,
                    request.OvertimeHours,
                    176 
                    })!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating overtime.");
            }

            return baseSalary + overtimePay;
        }
    }