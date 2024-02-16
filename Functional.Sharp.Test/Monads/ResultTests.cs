using Functional.Sharp.Errors;
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
}