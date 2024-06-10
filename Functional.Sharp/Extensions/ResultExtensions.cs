using Functional.Sharp.Errors;
using Functional.Sharp.Monads;

namespace Functional.Sharp.Extensions;

public static class ResultExtensions
{
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
}