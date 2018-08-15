using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public class CsvWriter : ICsvWriter
    {
        private TextWriter _writer;

        public CsvWriter(IFileProvider fileProvider, string filename)
        {
            if (fileProvider == null) throw new ArgumentNullException(nameof(fileProvider));
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            _writer = fileProvider.GetStreamWriter(filename);
        }

        public void Write(params string[] columns)
        {
            var outPut = "";

            for (var i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public void Dispose()
        {
            if (_writer == null)
                return;
            _writer.Dispose();
            _writer = null;
        }

        private void WriteLine(string line)
        {
            _writer.WriteLine(line);
        }
    }
}