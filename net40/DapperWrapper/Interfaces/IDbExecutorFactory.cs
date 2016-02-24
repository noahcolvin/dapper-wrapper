namespace DapperWrapper.Interfaces
{
    public interface IDbExecutorFactory
    {
        IDbExecutor CreateExecutor();
    }
}