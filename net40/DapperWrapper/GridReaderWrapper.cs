using Dapper;
using DapperWrapper.Interfaces;
using System;
using System.Collections.Generic;

namespace DapperWrapper
{
    public class GridReaderWrapper : IGridReader
    {
        private readonly SqlMapper.GridReader _gridReader;

        public GridReaderWrapper(SqlMapper.GridReader gridReader)
        {
            _gridReader = gridReader;
        }

        public IEnumerable<dynamic> Read(bool buffered = true)
        {
            return _gridReader.Read(buffered);
        }

        public IEnumerable<T> Read<T>(bool buffered = true)
        {
            return _gridReader.Read<T>(buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = "id")
        {
            return _gridReader.Read(func, splitOn);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = "id")
        {
            return _gridReader.Read(func, splitOn);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = "id")
        {
            return _gridReader.Read(func, splitOn);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn = "id")
        {
            return _gridReader.Read(func, splitOn);
        }

        public void Dispose()
        {
            _gridReader.Dispose();
        }
    }
}