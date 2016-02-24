using DapperWrapper.Interfaces;
using System.Transactions;

namespace DapperWrapper
{
    public class TransactionScopeWrapper : ITransactionScope
    {
        private readonly TransactionScope _transactionScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionScope">The transaction scope</param>
        public TransactionScopeWrapper(TransactionScope transactionScope)
        {
            _transactionScope = transactionScope;
        }

        #region Implementation of ITransactionScope

        public void Complete()
        {
            _transactionScope.Complete();
        }

        #endregion Implementation of ITransactionScope

        #region Implementation of IDisposable

        public void Dispose()
        {
            _transactionScope.Dispose();
        }

        #endregion Implementation of IDisposable
    }
}