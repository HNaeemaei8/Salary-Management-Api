using Application.Interfaces;
using EmployeeSalary.Application.Dto;
using System.Globalization;


namespace Infrastructure.Common
{
    public class CustomDataParser : IDataParser
    {
        public string SupportedFormat => "custom";

        public Task<List<SalaryCalculationRequest>> ParseAsync(string rawData)
        {
            var lines = rawData.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
                throw new Exception("Custom format must contain at least two lines.");

            var headers = lines[0].Split('/');
            var values = lines[1].Split('/');

            if (headers.Length != values.Length)
                throw new Exception("Headers and values count mismatch.");

            var dict = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
                dict[headers[i].Trim()] = values[i].Trim();

            var result = new SalaryCalculationRequest
            {
                EmployeeFirstName = dict["EmployeeFirstName"],
                EmployeeLastName = dict["EmployeeLastName"],
                SalaryDate = DateTime.Parse(dict["SalaryDate"], CultureInfo.InvariantCulture),
                BaseSalary = decimal.Parse(dict["BaseSalary"], CultureInfo.InvariantCulture),
                AttractionAllowance = decimal.Parse(dict["AttractionAllowance"], CultureInfo.InvariantCulture),
                TransportationAllowance = decimal.Parse(dict["TransportationAllowance"], CultureInfo.InvariantCulture),
                OverTimeCalculatorMethod = dict["OverTimeCalculatorMethod"],
                OvertimeHours = int.Parse(dict["OvertimeHours"])
            };

            return Task.FromResult(new List<SalaryCalculationRequest> { result });
        }
    }
}

