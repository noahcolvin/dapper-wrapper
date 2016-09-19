using DapperWrapper.Interfaces;
using System;
using System.Data.SqlClient;

namespace DapperWrapper
{
    public class SqlExecutorFactory : IDbExecutorFactory
    {
        #region Members

        private readonly string _connectionString;
        private readonly int _commandTimeout;

        #endregion Members

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection for the required database</param>
        /// <param name="commandTimeout">The default timeout to use for a connection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlExecutorFactory(string connectionString, int commandTimeout = 30)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _commandTimeout = commandTimeout;
        }

        /// <summary>
        /// Create an open database connection
        /// </summary>
        /// <returns>The newly created database connection</returns>
        public IDbExecutor CreateExecutor()
        {
            var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();

            return new SqlExecutor(dbConnection, _commandTimeout);
        }

        public void Dispose()
        {
        }
    }
}