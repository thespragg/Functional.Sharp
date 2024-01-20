namespace Functional.Sharp.Test;

public class MaybeTests
{
    [Fact]
    public void CreatingMaybe_ShouldReturnCorrectValue_Of()
    {
        var maybe = Maybe<int>.Of(0);
        Assert.True(maybe.HasValue);
        Assert.Equal(0, maybe.OrElse(-1));
    }
    
    [Fact]
    public void CreatingMaybe_ShouldReturnCorrectValue_None()
    {
        var maybe = Maybe<int>.None();
        Assert.False(maybe.HasValue);
    }

    [Fact]
    public void CreatingMaybe_ShouldReturnDefault_IfInitalisedWithNull()
    {
        var maybe = Maybe<int?>.Of(null);
        Assert.Equal(-1, maybe.OrElse(-1));
    }

    [Fact]
    public void FromMaybe_ShouldReturnCorrectMaybe()
    {
        var maybe = Maybe<int>.Of(1);
        var newMaybe = maybe.Map(x=>x.ToString());
        Assert.True(newMaybe.HasValue);
        Assert.Equal("1", newMaybe.OrElse("-1"));
    }
    
    [Fact]
    public void FromMaybe_ShouldReturnCorrectMaybe_ToMaybe()
    {
        var maybe = Maybe<int>.Of(1);
        var newMaybe = maybe.Map(x => x);
        Assert.True(newMaybe.HasValue);
        Assert.Equal(1, newMaybe.OrElse(-1));
    }

    [Fact]
    public void GetValueOrDefault_ReturnsDefault_WhenNoValue()
    {
        var maybe = Maybe<int>.None();
        var val = maybe.OrElse(-1);
        Assert.Equal(-1, val);
    }
    
    [Fact]
    public void GetValueOrDefault_ReturnsValue()
    {
        var maybe = Maybe<int>.Of(1);
        var val = maybe.OrElse(-1);
        Assert.Equal(1, val);
    }

    [Fact]
    public void Equals_ShouldEquateSameMaybe()
    {
        var maybe = Maybe<int>.Of(1);
        var eq = maybe.Equals(Maybe<int>.Of(1));
        Assert.True(eq);
    }
    
    [Fact]
    public void Equals_ShouldNotEquateDifferentMaybe()
    {
        var maybe = Maybe<int>.Of(1);
        var eq = maybe.Equals(Maybe<int>.Of(0));
        Assert.False(eq);
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnValueHashCode()
    {
        const int val = 100;
        var expectedHash = val.GetHashCode();
        var maybe = Maybe<int>.Of(val);
        Assert.Equal(expectedHash, maybe.GetHashCode());
    }
    
    [Fact]
    public void EqualOperator_ShouldCompareTwoMaybes()
    {
        var maybe1 = Maybe<int>.Of(1);
        var maybe2 = Maybe<int>.Of(1);
        Assert.True(maybe1 == maybe2);
    }
    
    [Fact]
    public void NotEqualOperator_ShouldCompareTwoMaybes()
    {
        var maybe1 = Maybe<int>.Of(1);
        var maybe2 = Maybe<int>.Of(2);
        Assert.True(maybe1 != maybe2);
    }
    
    [Fact]
    public void Parse_ReturnsCorrectObjects_Value()
    {
        int? val = 10;
        var maybe = Maybe<int>.Parse(val);
        Assert.True(maybe.HasValue);
        Assert.Equal(val, maybe.OrElse(-1));
        
        val = null;
        maybe = Maybe<int>.Parse(val);
        Assert.False(maybe.HasValue);
    }
    
    private record TestRecord();
}