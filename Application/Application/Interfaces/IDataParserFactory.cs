
namespace Application.Interfaces
{
    public interface IDataParserFactory
    {
        IDataParser GetParser(string format);
    }
}
