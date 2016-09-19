using System;

namespace DapperWrapper.Interfaces
{
    public interface IDbExecutorFactory : IDisposable
    {
        IDbExecutor CreateExecutor();
    }
}