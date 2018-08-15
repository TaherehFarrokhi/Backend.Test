using System;

namespace AddressProcessing.CSV
{
    public interface ICsvReader : IDisposable
    {
        Columns Read();

    }
}