using Dapper;
using DapperWrapper.Extensions;
using DapperWrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperWrapper
{
    /// <summary>
    /// Class to create a Sql Executor
    /// </summary>
    public class SqlExecutor : IDbExecutor
    {
        #region Members

        /// <summary>
        /// The SQL connection object
        /// </summary>
        private readonly SqlConnection _sqlConnection;
        private readonly int _commandTimeout;

        #endregion Members

        /// <summary>
        /// Constructor for the SQL Executor
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="commandTimeout">Default command timeout</param>
        public SqlExecutor(SqlConnection sqlConnection, int commandTimeout = 30)
        {
            _sqlConnection = sqlConnection;
            _commandTimeout = commandTimeout;
        }

        #region Sync Methods

        public int Execute(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _sqlConnection.Execute(
                sql,
                param,
                transaction,
                commandTimeout ?? _commandTimeout,
                commandType);
        }

        public IEnumerable<dynamic> Query(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _sqlConnection.Query(
                sql,
                param,
                transaction,
                buffered,
                commandTimeout ?? _commandTimeout,
                commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return SqlMapper.Query<TFirst, TSecond, TReturn>(_sqlConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout ?? _commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return SqlMapper.Query<T>(_sqlConnection, sql,
                param,
                transaction,
                buffered,
                commandTimeout ?? _commandTimeout,
                commandType);
        }

        public IGridReader QueryMultiple(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            var reader = _sqlConnection.QueryMultiple(sql, param, transaction, commandTimeout ?? _commandTimeout, commandType);
            return new GridReaderWrapper(reader);
        }

        #endregion Sync Methods

        #region Additional Extensions

        /// <summary>
        /// Perform the Query and then trim any strings that are returned
        /// NOTE: There could be a performance hit when doing this
        /// </summary>
        public IEnumerable<T> QueryAndTrimResults<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return AdditionalDapper.Query<T>(_sqlConnection, sql, param, transaction, buffered, commandTimeout ?? _commandTimeout, commandType);
        }

        /// <summary>
        /// Perform the Query and then trim any strings that are returned
        /// NOTE: There could be a performance hit when doing this
        /// </summary>
        public IEnumerable<TReturn> QueryAndTrimResults<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return AdditionalDapper.Query(_sqlConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout ?? _commandTimeout, commandType);
        }

        public int ExecuteProc(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Execute(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        public IEnumerable<dynamic> QueryProc(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true,
            int? commandTimeout = null)
        {
            return Query(sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure);
        }

        public IEnumerable<TReturn> QueryProc<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, CommandType.StoredProcedure);
        }

        public IEnumerable<T> QueryProc<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            return Query<T>(sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure);
        }

        public IGridReader QueryMultipleProc(string sql, object param = null, IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            return QueryMultiple(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);
        }

        #endregion Additional Extensions

        #region Async Methods

        public Task<int> ExecuteAsync(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _sqlConnection.ExecuteAsync(sql, param, transaction, commandTimeout ?? _commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _sqlConnection.QueryAsync(sql, param, transaction, commandTimeout ?? _commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return _sqlConnection.QueryAsync<T>(sql, param, transaction, commandTimeout ?? _commandTimeout, commandType);
        }

        public async Task<IGridReader> QueryMultipleAsync(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            var reader = await _sqlConnection.QueryMultipleAsync(sql, param, transaction, commandTimeout ?? _commandTimeout, commandType);
            return new GridReaderWrapper(reader);
        }

        #endregion Async Methods

        #region Implementation of IDisposable

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _sqlConnection.Dispose();
        }

        #endregion Implementation of IDisposable
    }
}