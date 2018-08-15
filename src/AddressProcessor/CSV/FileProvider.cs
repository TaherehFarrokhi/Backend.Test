using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public class FileProvider : IFileProvider
    {
        public TextReader GetStreamReader(string fileName)
        {
            ValidateFileName(fileName);

            return File.OpenText(fileName);
        }

        public TextWriter GetStreamWriter(string fileName)
        {
            ValidateFileName(fileName);

            var fileInfo = new FileInfo(fileName);
            return fileInfo.CreateText();
        }

        private static void ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName)) throw new FileNotFoundException(nameof(fileName));
        }
    }
}