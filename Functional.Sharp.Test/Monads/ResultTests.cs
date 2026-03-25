using Functional.Sharp.Errors;
using Functional.Sharp.Extensions;
using Functional.Sharp.Monads;

namespace Functional.Sharp.Test.Monads;

public class ResultTests
{
    [Fact]
    public void Success_CreatesAResult()
    {
        var sut = Result<int>.Success(100);
        sut.Match(
            success => Assert.Equal(100, success),
            err => throw new Exception("Wrong branch.")
        );
    }

    [Fact]
    public void Failure_CreatesAResult()
    {
        var sut = Result<int>.Failure(new NotFoundError("Resource not found"));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(NotFoundError), err.GetType())
        );
    }

    [Fact]
    public void MatchedSuccess_MatchesCorrectBranch()
    {
        var sut = Result<int>.Success(100);
        var matchedRes = sut.Match(
            success => success,
            err => -1
        );
        Assert.Equal(100, matchedRes);
    }

    [Fact]
    public void MatchedFailure_MatchesCorrectBranch()
    {
        var sut = Result<int>.Failure(new Error("Failed"));
        var matchedRes = sut.Match(
            success => success,
            err => -1
        );
        Assert.Equal(-1, matchedRes);
    }

    [Fact]
    public void OrElse_ReturnsSuccess_WhenSuccessful()
    {
        var sut = Result<int>.Success(100);
        var matchedRes = sut.OrElse(-1);
        Assert.Equal(100, matchedRes);
    }

    [Fact]
    public void OrElse_ReturnsElse_WhenFailure()
    {
        var sut = Result<int>.Failure(new Error("Failed"));
        var matchedRes = sut.OrElse(-1);
        Assert.Equal(-1, matchedRes);
    }

    [Fact]
    public void Map_ReturnsMappedSuccessfulResult()
    {
        var sut = Result<int>.Success(100);
        var mappedResult = sut.Map(res => (double)res);
        Assert.Equal(100.0, mappedResult.OrElse(-1));
        Assert.Equal(typeof(double), mappedResult.OrElse(-1).GetType());
    }

    [Fact]
    public void Map_ReturnsMappedFailedResult()
    {
        var sut = Result<int>.Failure(new Error("Failed"));
        var mappedResult = sut.Map(res => (double)res);
        Assert.Equal(-1, mappedResult.OrElse(-1));
        mappedResult.Match(
            success => throw new Exception("Wrong branch"),
            err => Assert.Equal(typeof(Error), err.GetType())
        );
    }

    [Fact]
    public void Implicit_WorksForSuccess()
    {
        Result<int> sut = 100;
        Assert.Equal(100, sut.OrElse(-1));
    }

    [Fact]
    public void Implicit_WorksForFailure()
    {
        Result<int> sut = new Error("Failed");
        Assert.Equal(-1, sut.OrElse(-1));
    }

    [Fact]
    public void Result_MapReturnsFlatResult()
    {
        Result<int> result = 1;
        var sut = result.Map(Result<int>.Success);
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public void Result_MapReturnsFlatResult_Error()
    {
        Result<int> result = 1;
        var sut = result.Map(x => Result<int>.Failure(new DatabaseError("Database error")));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }
    
    [Fact]
    public async Task Result_MapAsyncReturnsFlatResult()
    {
        Result<int> result = 1;
        var sut = await result.MapAsync(x => Task.FromResult(Result<int>.Success(x)));
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public async Task Result_MapAsyncReturnsFlatResult_Error()
    {
        Result<int> result = 1;
        var sut = await result.MapAsync(x => Task.FromResult(Result<int>.Failure(new DatabaseError("Database error"))));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }

    [Fact]
    public void FlatMap_ReturnsFlatSuccessResult()
    {
        Result<int> result = 1;
        var sut = result.FlatMap(Result<int>.Success);
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public void FlatMap_ReturnsFlatFailureResult()
    {
        Result<int> result = 1;
        var sut = result.FlatMap(x => Result<int>.Failure(new DatabaseError("Database error")));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }

    [Fact]
    public void FlatMap_PropagatesFailure()
    {
        Result<int> result = new Error("Failed");
        var sut = result.FlatMap(Result<int>.Success);
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal("Failed", err.Message)
        );
    }

    [Fact]
    public async Task FlatMapAsync_ReturnsFlatSuccessResult()
    {
        Result<int> result = 1;
        var sut = await result.FlatMapAsync(x => Task.FromResult(Result<int>.Success(x)));
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public async Task FlatMapAsync_ReturnsFlatFailureResult()
    {
        Result<int> result = 1;
        var sut = await result.FlatMapAsync(x => Task.FromResult(Result<int>.Failure(new DatabaseError("Database error"))));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal(typeof(DatabaseError), err.GetType())
        );
    }

    [Fact]
    public async Task FlatMapAsync_PropagatesFailure()
    {
        Result<int> result = new Error("Failed");
        var sut = await result.FlatMapAsync(x => Task.FromResult(Result<int>.Success(x)));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal("Failed", err.Message)
        );
    }

    [Fact]
    public async Task TaskFlatMapAsync_WithSyncMapper_ReturnsFlatSuccessResult()
    {
        Task<Result<int>> task = Task.FromResult(Result<int>.Success(1));
        var sut = await task.FlatMapAsync(Result<int>.Success);
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public async Task TaskFlatMapAsync_WithSyncMapper_PropagatesFailure()
    {
        Task<Result<int>> task = Task.FromResult(Result<int>.Failure(new Error("Failed")));
        var sut = await task.FlatMapAsync(Result<int>.Success);
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal("Failed", err.Message)
        );
    }

    [Fact]
    public async Task TaskFlatMapAsync_WithAsyncMapper_ReturnsFlatSuccessResult()
    {
        Task<Result<int>> task = Task.FromResult(Result<int>.Success(1));
        var sut = await task.FlatMapAsync(x => Task.FromResult(Result<int>.Success(x)));
        Assert.Equal(1, sut.OrElse(-1));
    }

    [Fact]
    public async Task TaskFlatMapAsync_WithAsyncMapper_PropagatesFailure()
    {
        Task<Result<int>> task = Task.FromResult(Result<int>.Failure(new Error("Failed")));
        var sut = await task.FlatMapAsync(x => Task.FromResult(Result<int>.Success(x)));
        sut.Match(
            success => throw new Exception("Wrong branch."),
            err => Assert.Equal("Failed", err.Message)
        );
    }
}