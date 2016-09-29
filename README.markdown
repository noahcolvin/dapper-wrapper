DapperWrapper is a library that wraps the [Dapper](https://github.com/StackExchange/dapper-dot-net) extension methods on `IDbConnection` to make unit testing easier.

Why bother? Because stubbing the extension methods used in a method-under-unit-test is not simple. For instance, you can't just use a library like [Moq](https://github.com/moq/moq4) or [NSubstitute](http://nsubstitute.github.io/) to stub the `.Query` extension method on a fake `IDbConnection`. To work around this, this library introduces a new abstraction, `IDbExecutor`.

## The `IDbExecutor` Interface

The `IDbExectuor` interface has many methods, each corresponding to a Dapper extension method: `Execute`, `Query`, `Query<T>`, `QueryMultiple`, `QueryMultiple<T>`, etc.. Wherever you would previously inject an `IDbConnection` to use with Dapper, you instead inject an `IDbExecutor`. There is a single implementation of `IDbExecutor` included in DapperWrapper, `SqlExecutor`, that uses the Dapper extension methods against `SqlConnection`. Adding your own `IDbExecutor` against other implementations of `IDbConnection` is easy.

This version allows specifying a command timeout when creating the SqlExecutor to use in each command if a different value is not specified. Each of the extension methods above also include one ending in `Proc` which will automatically assume the command is of type `CommandType.StoredProcedure`.

Example use of `IDbExecutor`:

```
public IEnumerable<SemanticVersion> GetAllPackageVersions(
  string packageId,
  IDbExecutor dbExecutor) {
  return dbExecutor.Query<string>("SELECT p.version FROM packages p WHERE p.id = @packageId", new { packageId })
	.Select(version => new SemanticVersion(version));
}
``` 

## Injecting `IDbExecutor`

You probably already have an apporach to injecting `IDbConnection` into your app that you're happy with. That same approach will probably work just as well with `IDbExecutor`. 

Personally, in my dependency container or service locator, I like to bind `IDbExecutor` to a method that instantiates a new `SqlConnection` and `SqlExecutor`. If you need to control the creation of the executor (for instance, you only need it conditionally), you could bind `Func<IDbExecutor>`. There's also an `IDbExecutorFactory` interface in DapperWrapper you could use, but it comes with the same downsides as any factory type.

Example of binding `IDbExecutor` and `IDbExecutorFactory` using Ninject:

```
public class DependenciesRegistrar : NinjectModule {
  public override void Load() {
	Bind<IDbExecutor>()
	  .ToMethod(context => {
		var sqlConnection = new SqlConnection(connectionString);
		sqlConnection.Open();
		return new SqlExecutor(sqlConnection);
	  });
	Bind<IDbExecutorFactory>()
	  .ToMethod(context => {
		return new SqlExecutorFactory(connectionString);
	  })
	  .InSingletonScope();
  }
}
```

## Transactions

Sometimes there is a need to assert whether a method-under-unit-test completes a transaction via `TransactionScope`. To make this easier, DapperWrapper also has an `ITransactionScope` interface (and `TransactionScopeWrapper` implementation) that makes it easy to create a fake transaction, and stub (and assert on) the `Complete` method. As with `IDbExecutor`, you can bind it directly, via `Func<ITransactionScope>`.

## Additional
There are also times when the data coming from the database is not trimmed and so DapperWrapper includes `QueryAndTrimResults<T>` for this purpose.

## Versions
### 1.1.2
Updated for [Dapper.SimpleCRUD](https://github.com/ericdc1/Dapper.SimpleCRUD) 1.13.0.

### 1.1.1
Added `IDisposable`.

### 1.1.0
Added wrapper methods for [Dapper.SimpleCRUD](https://github.com/ericdc1/Dapper.SimpleCRUD) to enhance testability and to use the same pattern as other `Dapper` methods. If you do not need `Dapper.SimpleCRUD` support you can stick with version 1.0.1; that code has not been modified.

### 1.0.1
Included `Proc` methods to assume `CommandType.StoredProcedure` and defaulting the `CommandTimeout`.