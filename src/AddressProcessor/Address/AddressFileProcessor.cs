using System;
using System.Collections.Generic;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;

namespace AddressProcessing.Address
{
    public class AddressFileProcessor
    {
        private readonly IMailShot _mailShot;
        private readonly ICsvReaderWriterBuilder _builder;

        public AddressFileProcessor(IMailShot mailShot)
        {
            if (mailShot == null) throw new ArgumentNullException("mailShot");
            _mailShot = mailShot;
        }

        public void Process(string inputFile)
        {
            var reader = new CSVReaderWriter();
            reader.Open(inputFile, CSVReaderWriter.Mode.Read);

            var addressRecordProcessor = new AddressRecordProcessor();

            string column1, column2;
            var addressRecoredStore = new Dictionary<string, AddressRecord>();

            while(reader.Read(out column1, out column2))
            {
                if (addressRecoredStore.ContainsKey(column1))
                {
                    // Log warning for duplicate
                    continue;
                }

                var address = addressRecordProcessor.Process(column2);
                addressRecoredStore.Add(column1, address);
                
                
            }

            foreach (var key in addressRecoredStore.Keys)
            {
                _mailShot.SendMailShot(key, addressRecoredStore[key]);
            }

            reader.Close();
        }
    }

    public class AddressRecordProcessor : IAddressRecordProcessor
    {
        public AddressRecord Process(string recordText)
        {
            if (recordText == null) throw new ArgumentNullException(nameof(recordText));

            var splits = recordText.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length != 5)
                throw new InvalidRecordException($"Invalid address line. It should be 5 section but it's only contains {splits.Length}");

            return new AddressRecord()
            {
                Address = splits[0],
                City = splits[1],
                Province = splits[2],
                PostCode = splits[3],
                Country = splits[4]
            };
        }
    }

    public class InvalidRecordException : Exception
    {
        public InvalidRecordException(string message): base(message)
        {
        }
    }

    public interface IAddressRecordProcessor
    {
        AddressRecord Process(string recordText);
    }
}
