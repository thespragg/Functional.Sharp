using Functional.Sharp.Errors;
using Functional.Sharp.Helpers;
using Functional.Sharp.Monads;

namespace Functional.Sharp.Test.Helpers;

public class TryTests
{
    [Fact]
    public void Execute_ShouldExecute_WhenNotThrown()
    {
        var sut = Try.Execute(() => 5);
        Assert.Equal(5, sut.OrElse(-1));
    }

    [Fact]
    public void Execute_ShouldReturnError_WhenThrown()
    {
        var sut = Try.Execute<int>(() => throw new Exception("Error"));
        Assert.Equal(-1, sut.OrElse(-1));
    }

    [Fact]
    public void Execute_ShouldExecuteFinally()
    {
        var finallyExecuted = false;
        var sut = Try.Execute(() => 5, onFinally: () => finallyExecuted = true);
        Assert.Equal(5, sut.OrElse(-1));
        Assert.True(finallyExecuted);
    }
    
    [Fact]
    public void Execute_ShouldReturnMappedError()
    {
        var sut = Try.Execute<int>(
            () => throw new Exception("Error"),
            ex => new NotFoundError(ex.Message)
        );

        sut.Match(
            success => throw new Exception("Wrong branch"),
            err => Assert.Equal(typeof(NotFoundError), err.GetType())
        );
    }

    [Fact]
    public void Execute_ShouldReturnBoolean_WhenNoReturn()
    {
        var sut = Try.Execute(() => { _ = 5 * 5; });
        Assert.True(sut);
        sut = Try.Execute(() => throw new Exception("Failed"));
        Assert.False(sut);
    }

    [Fact]
    public async void ExecuteAsync_ShouldHandleAsync_Successful()
    {
        var sut = await Try.ExecuteAsync(() => Task.FromResult(5));
        Assert.Equal(5, sut.OrElse(-1));
    }
    
    [Fact]
    public async void ExecuteAsync_ShouldHandleAsync_Thrown()
    {
        var sut = await Try.ExecuteAsync<int>(() => throw new Exception("Failed"));
        Assert.Equal(-1, sut.OrElse(-1));
    }
    
    [Fact]
    public async void ExecuteAsync_ShouldHandleAsyncFinally()
    {
        var finallyExecuted = false;
        var sut = await Try.ExecuteAsync(() => Task.FromResult(5), onFinally: () => finallyExecuted = true);
        Assert.Equal(5, sut.OrElse(-1));
        Assert.True(finallyExecuted);
    }
    
    [Fact]
    public async void ExecuteAsync_ShouldHandleAsyncBooleanReturn()
    {
        var sut = await Try.ExecuteAsync(() =>
        {
            _ = 5 * 5;
            return Task.CompletedTask;
        });
        Assert.True(sut);
        sut = await Try.ExecuteAsync(() => throw new Exception("Failed"));
        Assert.False(sut);
    }
    
    [Fact]
    public void Execute_ShouldHandleNestedResult()
    {
        var sut = Try.Execute(() => Result<int>.Success(1));
        Assert.Equal(1, sut.OrElse(-1));
    }
    
    [Fact]
    public void Execute_ShouldHandleNestedResult_Error()
    {
        var sut = Try.Execute(() => Result<int>.Failure(new DatabaseError("Database error")));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }
    
    [Fact]
    public void Execute_ShouldHandleNestedResult_Exception()
    {
        var sut = Try.Execute<Result<int>>(() => throw new Exception("Database Error"), ex => new DatabaseError(ex.Message));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldHandleNestedResult_Error()
    {
        var sut = await Try.ExecuteAsync(async () => await Task.FromResult(Result<int>.Failure(new DatabaseError("Database error"))));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldHandleNestedResult()
    {
        var sut = await Try.ExecuteAsync(async () => await Task.FromResult(Result<int>.Success(1)));
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleNestedResult_Exception()
    {
        var sut = await Try.ExecuteAsync<Result<int>>(() => throw new Exception("Database Error"), ex => new DatabaseError(ex.Message));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }
}