using Application.Interfaces;
using Application.Dto;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Infrastructure.Infrastucture.Common
{
    public class XmlDataParser : IDataParser
    {
        public string SupportedFormat => "xml";

        public Task<List<SalaryCalculationRequest>> ParseAsync(string rawData)
        {
            var serializer = new XmlSerializer(typeof(SalaryCalculationRequest));
            using var stringReader = new StringReader(rawData);

            var record = (SalaryCalculationRequest?)serializer.Deserialize(stringReader);

            if (record == null)
                throw new Exception("Invalid XML data.");

            return Task.FromResult(new List<SalaryCalculationRequest> { record });

        }
    }
}

