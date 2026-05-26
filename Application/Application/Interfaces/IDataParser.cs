using Application.Dto;

namespace Application.Interfaces
{
        public interface IDataParser
        {
            string SupportedFormat { get; }
            Task<List<SalaryCalculationRequest>> ParseAsync(string rawData);
        }
    }

