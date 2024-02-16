using Functional.Sharp.Helpers;

namespace Functional.Sharp.Test.Helpers;

public class MemoizationTests
{
    [Fact]
    public void Memoize_ShouldCacheResult()
    {
        var count = 0;
        var memoizedFunc = Memoization.Memoize<int, int>(x =>
        {
            count++;
            return x * x;
        });

        var result1 = memoizedFunc(5);
        var result2 = memoizedFunc(5);

        Assert.Equal(1, count);
        Assert.Equal(25, result1);
        Assert.Equal(25, result2);
    }

    [Fact]
    public void Memoize_ShouldHandleDifferentInputs()
    {
        var count = 0;
        var memoizedFunc = Memoization.Memoize<int, int>(x =>
        {
            count++;
            return x * x;
        });

        var result1 = memoizedFunc(5);
        var result2 = memoizedFunc(10);

        Assert.Equal(2, count);
        Assert.Equal(25, result1);
        Assert.Equal(100, result2);
    }

    [Fact]
    public void Memoize_ShouldWorkWithComplexTypes()
    {
        var count = 0;
        var memoizedFunc = Memoization.Memoize<TestClass, int>(
            x =>
            {
                count++;
                return x.Property * count;
            },
            (c1, c2) => c1.Property == c2.Property,
            c => c.Property.GetHashCode()
        );

        var result1 = memoizedFunc(new TestClass { Property = 5 });
        var result2 = memoizedFunc(new TestClass { Property = 5 });

        Assert.Equal(1, count);
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
    }

    private class TestClass
    {
        public int Property { get; init; }
    };
}