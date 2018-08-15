using System.IO;

namespace AddressProcessing.CSV
{
    public interface IFileProvider
    {
        TextReader GetStreamReader(string fileName);
        TextWriter GetStreamWriter(string fileName);
    }
}