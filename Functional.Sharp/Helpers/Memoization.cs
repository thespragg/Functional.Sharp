namespace Functional.Sharp.Helpers;

public static class Memoization
{
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
        where T : notnull => Memoize(func, null, null);

    public static Func<T, TResult> Memoize<T, TResult>(
        Func<T, TResult> func,
        Func<T, T, bool>? equalityCheck,
        Func<T, int>? getHashCode
    ) where T : notnull
    {
        IEqualityComparer<T> comparer = equalityCheck is null || getHashCode is null
            ? EqualityComparer<T>.Default
            : new DelegateComparer<T>(equalityCheck, getHashCode);
        
        var cache = new Dictionary<T, TResult>(comparer);
        return arg =>
        {
            if (cache.TryGetValue(arg, out var result)) return result;
            result = func(arg);
            cache[arg] = result;
            return result;
        };
    }

    private class DelegateComparer<T>(
        Func<T, T, bool> equals,
        Func<T, int> getHashCode
    ) : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
            => equals(x, y);

        public int GetHashCode(T obj)
            => getHashCode(obj);
    }
}