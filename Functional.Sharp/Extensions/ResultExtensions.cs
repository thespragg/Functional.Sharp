using Functional.Sharp.Errors;
using Functional.Sharp.Monads;

namespace Functional.Sharp.Extensions;

public static class ResultExtensions
{
    public static IEnumerable<T> StripFailures<T>(this IEnumerable<Result<T>> results)
    {
        var res = new List<T>();
        foreach (var result in results)
        {
            result.OnSuccess(val => res.Add(val));
        }

        return res;
    }

    public static Result<IEnumerable<T>> ToSingleResult<T>(this IEnumerable<Result<T>> results)
    {
        var res = new List<T>();
        foreach (var result in results)
        {
            var (s, e) = result.Match<(T?, Error?)>(val => (val, null), err => (default, err));
            if (s is not null) res.Add(s);
            else return e!;
        }

        return res;
    }

    public static async Task<Result<TNext>> MapAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNext>> next)
    {
        var result = await task;
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(next);

        return await result.MapAsync(
            async value => await next(value)
        );
    }

    public static async Task<Result<TNext>> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNext>> success,
        Func<Error, TNext> failure)
    {
        var result = await task;

        return await result.MatchAsync(
            async value => await success(value),
            failure
        );
    }

    public static async Task<Result<TNext>> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, TNext> success,
        Func<Error, TNext> failure)
    {
        var result = await task;

        return await result.MatchAsync(
            value => Task.FromResult(success(value)),
            failure
        );
    }

    public static async Task<Result<TValue>> OnSuccessAsync<TValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task> onSuccess)
    {
        var result = await task;
        return await result.OnSuccessAsync(onSuccess);
    }

    public static async Task<Result<TValue>> OnFailureAsync<TValue>(
        this Task<Result<TValue>> task,
        Func<Error, Task> onFailure)
    {
        var result = await task;
        return await result.OnFailureAsync(onFailure);
    }

    public static Result<T> Flatten<T>(this Result<Result<T>> result)
        => result.Match(
            success => success.Map(v => v),
            failure => failure
        );

    public static async Task<Result<T>> FlattenAsync<T>(
        this Task<Result<Result<T>>> result
    ) => (await result).Match(
        success => success.Map(v => v),
        failure => failure
    );

    public static Result<(T1, T2)> Combine<T1, T2>(
        this (Result<T1>, Result<T2>) tuple
    ) => tuple.Item1.Map(
        one => tuple.Item2.Map(two => (one, two))
    );

    public static Result<(T1, T2, T3)> Combine<T1, T2, T3>(
        this (Result<T1>, Result<T2>, Result<T3>) tuple
    ) => tuple.Item1.Map(
        one => tuple.Item2.Map(
            two => tuple.Item3.Map(three => (one, two, three))
        )
    );

    public static Result<(T1, T2, T3, T4)> Combine<T1, T2, T3, T4>(
        this (Result<T1>, Result<T2>, Result<T3>, Result<T4>) tuple
    ) => tuple.Item1.Map(
        one => tuple.Item2.Map(
            two => tuple.Item3.Map(
                three => tuple.Item4.Map(four => (one, two, three, four)
                )
            )
        )
    );

    public static Result<(T1, T2, T3, T4, T5)> Combine<T1, T2, T3, T4, T5>(
        this (Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>) tuple
    ) => tuple.Item1.Map(
        one => tuple.Item2.Map(
            two => tuple.Item3.Map(
                three => tuple.Item4.Map(
                    four => tuple.Item5.Map(five => (one, two, three, four, five)
                    )
                )
            )
        )
    );
}