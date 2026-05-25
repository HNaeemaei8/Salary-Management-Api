using Application.Interfaces;
using EmployeeSalary.Application.Dto;
using System.Text.Json;

namespace Infrastucture.Common
{
    namespace Infrastructure
    {
        public class JsonDataParser : IDataParser
        {
            public string SupportedFormat => "json";

            public Task<List<SalaryCalculationRequest>> ParseAsync(string rawData)
            {
                var records = JsonSerializer.Deserialize<List<SalaryCalculationRequest>>(rawData);

                if (records == null || records.Count == 0)
                {
                    throw new Exception("Invalid or empty JSON data.");
                }

                return Task.FromResult(records);

            }
        }
    }
}