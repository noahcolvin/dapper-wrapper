using DapperWrapper.Interfaces;
using System;
using System.Data.SqlClient;

namespace DapperWrapper
{
    public class SqlExecutorFactory : IDbExecutorFactory
    {
        #region Members

        private readonly string _connectionString;

        #endregion Members

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection for the required database</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SqlExecutorFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            _connectionString = connectionString;
        }

        /// <summary>
        /// Create an open database connection
        /// </summary>
        /// <returns>The newly created database connection</returns>
        public IDbExecutor CreateExecutor()
        {
            var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();

            return new SqlExecutor(dbConnection);
        }
    }
}