using Dapper;
using DapperWrapper.Extensions;
using DapperWrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            return SqlMapper.Query(_sqlConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout ?? _commandTimeout, commandType);
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

        #region SimpleCRUD Extensions

        public string GetDialect()
        {
            return SimpleCRUD.GetDialect();
        }

        public void SetDialect(SimpleCRUD.Dialect dialect)
        {
            SimpleCRUD.SetDialect(dialect);
        }

        public void SetTableNameResolver(SimpleCRUD.ITableNameResolver resolver)
        {
            SimpleCRUD.SetTableNameResolver(resolver);
        }

        public void SetColumnNameResolver(SimpleCRUD.IColumnNameResolver resolver)
        {
            SimpleCRUD.SetColumnNameResolver(resolver);
        }

        public T Get<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Get<T>(id, transaction, commandTimeout ?? _commandTimeout);
        }

        public IEnumerable<T> GetList<T>(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.GetList<T>(whereConditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public IEnumerable<T> GetList<T>(string conditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.GetList<T>(conditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public IEnumerable<T> GetList<T>()
        {
            return _sqlConnection.GetList<T>();
        }

        public IEnumerable<T> GetListPaged<T>(int pageNumber, int rowsPerPage, string conditions, string @orderby, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.GetListPaged<T>(pageNumber, rowsPerPage, conditions, @orderby, transaction, commandTimeout ?? _commandTimeout);
        }

        public int? Insert(object entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Insert(entityToInsert, transaction, commandTimeout ?? _commandTimeout);
        }

        public TKey Insert<TKey>(object entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Insert<TKey>(entityToInsert, transaction, commandTimeout ?? _commandTimeout);
        }

        public int Update(object entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Update(entityToUpdate, transaction, commandTimeout ?? _commandTimeout);
        }

        public int Delete<T>(T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Delete(entityToDelete, transaction, commandTimeout ?? _commandTimeout);
        }

        public int Delete<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.Delete<T>(id, transaction, commandTimeout ?? _commandTimeout);
        }

        public int DeleteList<T>(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.DeleteList<T>(whereConditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public int DeleteList<T>(string conditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.DeleteList<T>(conditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public int RecordCount<T>(string conditions = "", IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.RecordCount<T>(conditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public int RecordCount<T>(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return _sqlConnection.RecordCount<T>(whereConditions, transaction, commandTimeout ?? _commandTimeout);
        }

        public Guid SequentialGuid()
        {
            return SimpleCRUD.SequentialGuid();
        }

        #endregion

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