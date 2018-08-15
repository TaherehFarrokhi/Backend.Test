using System;

namespace AddressProcessing.CSV
{
    public class CsvReaderWriterBuilder : ICsvReaderWriterBuilder
    {
        private readonly IFileProvider _fileProvider;

        public CsvReaderWriterBuilder(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        public ICsvReader BuildReader(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            return new CsvReader(_fileProvider, filename);
        }

        public ICsvWriter BuildWriter(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            return new CsvWriter(_fileProvider, filename);
        }
    }
}