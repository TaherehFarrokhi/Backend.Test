using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public class CsvReader : ICsvReader
    {
        private TextReader _reader;

        public CsvReader(IFileProvider fileProvider, string filename)
        {
            if (fileProvider == null) throw new ArgumentNullException(nameof(fileProvider));
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            _reader = fileProvider.GetStreamReader(filename);
        }

        public Columns Read()
        {
            char[] separator = { '\t' };

            var line = ReadLine();

            if (line == null)
                return null;

            var columns = line.Split(separator);

            return columns.Length == 0 ? null : new Columns(columns[Constants.FirstColumn], columns[Constants.SecondColumn]);
        }

        public void Dispose()
        {
            if (_reader == null)
                return;

            _reader.Dispose();
            _reader = null;
        }

        private string ReadLine()
        {
            return _reader.ReadLine();
        }
    }
}