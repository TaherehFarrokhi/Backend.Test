namespace AddressProcessing.CSV
{
    public interface ICsvReaderWriterBuilder
    {
        ICsvReader BuildReader(string filename);
        ICsvWriter BuildWriter(string filename);
    }
}