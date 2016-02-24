using System;

namespace DapperWrapper.Interfaces
{
    public interface ITransactionScope : IDisposable
    {
        void Complete();
    }
}