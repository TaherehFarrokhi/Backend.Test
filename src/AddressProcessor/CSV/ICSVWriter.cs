using System;

namespace AddressProcessing.CSV
{
    public interface ICsvWriter: IDisposable
    {
        void Write(params string[] columns);
    }
}