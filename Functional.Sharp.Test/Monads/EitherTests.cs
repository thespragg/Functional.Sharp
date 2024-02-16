using Functional.Sharp.Monads;

namespace Functional.Sharp.Test.Monads;

public class EitherTests
{
    [Fact]
    public void EitherLeft_ShouldCreateLeft()
    {
        var either = Either<int, string>.Left(1);
        either.Match(
            x => Assert.Equal(1, x),
            _ => throw new Exception("Wrong branch"));
    }
    
    [Fact]
    public void EitherRight_ShouldCreateRight()
    {
        var either = Either<int, string>.Right("1");
        either.Match(
            _ => throw new Exception("Wrong branch") ,
            y => Assert.Equal("1", y));
    }
    
    [Fact]
    public void EitherLeft_MatchReturns()
    {
        var either = Either<int, string>.Left(1);
        var val = either.Match(
            x => 1,
            _ => -1);
        Assert.Equal(1, val);
    }
    
    [Fact]
    public void EitherRight_MatchReturns()
    {
        var either = Either<string, int>.Right(1);
        var val = either.Match(
            x => -1,
            _ => 1);
        Assert.Equal(1, val);
    }
}