using System;
using System.Collections.Generic;

namespace DapperWrapper.Interfaces
{
    public interface IGridReader : IDisposable
    {
        IEnumerable<dynamic> Read(bool buffered = true);

        IEnumerable<T> Read<T>(bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = "id");

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = "id");

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = "id");

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn = "id");
    }
}