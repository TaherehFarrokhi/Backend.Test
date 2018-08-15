using System;
using System.Collections;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : IDisposable
    {
        private readonly ICsvReaderWriterBuilder _builder;

        private ICsvReader _csvReader;
        private ICsvWriter _csvWriter;

        public CSVReaderWriter()
        {
            // This needs to be injected by IoC container but as I didn't wanted to change the signature of the caller class, I didn't pass it through constructor
            var fileProvider = new FileProvider();
            _builder = new CsvReaderWriterBuilder(fileProvider);
        }

        // It's anti-pattern to have two constructors (one with injection and one without) but this is done for the purpose of the mocking for the tests.
        public CSVReaderWriter(ICsvReaderWriterBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _csvReader = _builder.BuildReader(fileName);
                    break;
                case Mode.Write:
                    _csvWriter = _builder.BuildWriter(fileName);
                    break;
                default:
                    throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            if(_csvWriter == null)
                throw new ApplicationException("File is closed or hasn't been opened in write mode.");

            _csvWriter.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            if (_csvReader == null)
                throw new ApplicationException("File is closed or hasn't been opened in read mode.");

            var columns = _csvReader.Read();
            if (columns == null)
                return false;

            column1 = columns.Column1;
            column2 = columns.Column2;

            return true;
        }

        public bool Read(out string column1, out string column2)
        {
            if (_csvReader == null)
                throw new ApplicationException("File is closed or hasn't been opened in read mode.");

            var columns = _csvReader.Read();
            if (columns == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            column1 = columns.Column1;
            column2 = columns.Column2;

            return true;

        }

        public void Close()
        {
            if (_csvWriter != null)
            {
                _csvWriter.Dispose();
                _csvWriter = null;
            }

            if (_csvReader == null)
                return;

            _csvReader.Dispose();
            _csvReader = null;

        }

        public void Dispose()
        {
            Close();
        }
    }
}
