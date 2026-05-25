using Application.Interfaces;

namespace Infrastructure.Common
{
    public class DataParserFactory :IDataParserFactory
    {
        private readonly IEnumerable<IDataParser> _parsers;

        public DataParserFactory(IEnumerable<IDataParser> parsers)
        {
            _parsers = parsers;
        }

        public IDataParser GetParser(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format), "Input format cannot be null.");
            }

            var parser = _parsers.FirstOrDefault(p =>
                p.SupportedFormat.Equals(format, StringComparison.OrdinalIgnoreCase));

            if (parser == null)
            {
                throw new KeyNotFoundException($"Format '{format}' is not supported.");
            }

            return parser;
        }
    }
}
