namespace Functional.Sharp.Test;

public class MaybeTests
{
    [Fact]
    public void CreatingMaybe_ShouldReturnCorrectValue_Some()
    {
        var maybe = Maybe<int>.Some(0);
        Assert.True(maybe.HasValue);
        Assert.Equal(0, maybe.Value);
    }
    
    [Fact]
    public void CreatingMaybe_ShouldReturnCorrectValue_None()
    {
        var maybe = Maybe<int>.None;
        Assert.False(maybe.HasValue);
    }

    [Fact]
    public void CreatingMaybe_ShouldThrow_IfValueNull()
    {
        var ex = Record.Exception(() => Maybe<int?>.Some(null!));
        Assert.NotNull(ex);
    }
    
    [Fact]
    public void AccessingValue_ShouldThrow_WhenNoValue()
    {
        var maybe = Maybe<int>.None;
        var ex = Record.Exception(() => maybe.Value);
        Assert.NotNull(ex);
    }

    [Fact]
    public void FromMaybe_ShouldReturnCorrectMaybe()
    {
        var maybe = Maybe<int>.Some(1);
        var newMaybe = Maybe<string>.FromMaybe(maybe, i => i.ToString());
        Assert.True(newMaybe.HasValue);
        Assert.Equal("1", newMaybe.Value);
    }
    
    [Fact]
    public void FromMaybe_ShouldReturnCorrectMaybe_ToMaybe()
    {
        var maybe = Maybe<int>.Some(1);
        var newMaybe = Maybe<string>.FromMaybe(maybe, Maybe<int>.Some);
        Assert.True(newMaybe.HasValue);
        Assert.Equal(1, newMaybe.Value);
    }

    [Fact]
    public void GetValueOrDefault_ReturnsDefault_WhenNoValue()
    {
        var maybe = Maybe<int>.None;
        var val = maybe.GetValueOrDefault(-1);
        Assert.Equal(-1, val);
    }
    
    [Fact]
    public void GetValueOrDefault_ReturnsValue()
    {
        var maybe = Maybe<int>.Some(1);
        var val = maybe.GetValueOrDefault(-1);
        Assert.Equal(1, val);
    }

    [Fact]
    public void Equals_ShouldEquateSameMaybe()
    {
        var maybe = Maybe<int>.Some(1);
        var eq = maybe.Equals(Maybe<int>.Some(1));
        Assert.True(eq);
    }
    
    [Fact]
    public void Equals_ShouldNotEquateDifferentMaybe()
    {
        var maybe = Maybe<int>.Some(1);
        var eq = maybe.Equals(Maybe<int>.Some(0));
        Assert.False(eq);
    }
    
    [Fact]
    public void Equals_ShouldEquateBaseType()
    {
        var maybe = Maybe<int>.Some(1);
        var eq = maybe.Equals(1);
        Assert.True(eq);
    }
    
    [Fact]
    public void Equals_ShouldNotEquateOtherTypes()
    {
        var maybe = Maybe<int>.Some(1);
        // ReSharper disable once SuspiciousTypeConversion.Global
        var eq = maybe.Equals("1");
        Assert.False(eq);
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnValueHashCode()
    {
        const int val = 100;
        var expectedHash = val.GetHashCode();
        var maybe = Maybe<int>.Some(val);
        Assert.Equal(expectedHash, maybe.GetHashCode());
    }
    
    [Fact]
    public void GetHashCode_ShouldReturnZero_WhenNoValue()
    {
        var maybe = Maybe<int>.None;
        Assert.Equal(0, maybe.GetHashCode());
    }

    [Fact]
    public void EqualOperator_ShouldCompareTwoMaybes()
    {
        var maybe1 = Maybe<int>.Some(1);
        var maybe2 = Maybe<int>.Some(1);
        Assert.True(maybe1 == maybe2);
    }
    
    [Fact]
    public void EqualOperator_ShouldCompareToValue()
    {
        var maybe1 = Maybe<int>.Some(1);
        Assert.True(maybe1 == 1);
    }
    
    [Fact]
    public void NotEqualOperator_ShouldCompareTwoMaybes()
    {
        var maybe1 = Maybe<int>.Some(1);
        var maybe2 = Maybe<int>.Some(2);
        Assert.True(maybe1 != maybe2);
    }
    
    [Fact]
    public void NotEqualOperator_ShouldCompareToValue()
    {
        var maybe1 = Maybe<int>.Some(1);
        Assert.True(maybe1 != 2);
    }
    
    [Fact]
    public void Parse_ReturnsCorrectObjects_Value()
    {
        int? val = 10;
        var maybe = Maybe<int>.Parse(val);
        Assert.True(maybe.HasValue);
        Assert.Equal(val, maybe.Value);
        
        val = null;
        maybe = Maybe<int>.Parse(val);
        Assert.False(maybe.HasValue);
    }
    
    [Fact]
    public void Parse_ReturnsCorrectObjects_Reference()
    {
        var val = new TestRecord();
        var maybe = Maybe<TestRecord>.Parse(val);
        Assert.True(maybe.HasValue);
        
        val = null;
        maybe = Maybe<TestRecord>.Parse(val);
        Assert.False(maybe.HasValue);
    }
    
    private record TestRecord();
}