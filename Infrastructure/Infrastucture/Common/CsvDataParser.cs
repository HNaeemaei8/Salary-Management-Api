using Application.Dto;
using Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Infrastructure.Common
{
    public class CsvDataParser : IDataParser
    {
        public string SupportedFormat => "csv";

        public Task<List<SalaryCalculationRequest>> ParseAsync(string rawData)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8,
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using var reader = new StringReader(rawData);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<SalaryCalculationRequest>().ToList();

            if (records == null || records.Count == 0)
                throw new Exception("Invalid or empty CSV data.");

            return Task.FromResult(records);
        }
    }
}
