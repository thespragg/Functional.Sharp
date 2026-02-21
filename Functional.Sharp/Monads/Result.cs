using Functional.Sharp.Errors;
using JetBrains.Annotations;

namespace Functional.Sharp.Monads;

[PublicAPI]
public class Result<TValue> : IResult<Result<TValue>>
{
    private readonly TValue? _value;
    private readonly bool _isSuccess;
    private readonly Error? _error;

    private Result(TValue value)
        => (_value, _isSuccess) = (value, true);

    private Result(Error error)
        => (_error, _isSuccess) = (error, false);

    public static Result<TValue> Success(TValue value)
        => new(value);

    public static Result<TValue> Failure(Error error)
        => new(error);

    public T Match<T>(
        Func<TValue, T> success,
        Func<Error, T> error
    ) => _isSuccess ? success(_value!) : error(_error!);

    public async Task<T> MatchAsync<T>(
        Func<TValue, Task<T>> success,
        Func<Error, T> error
    ) => _isSuccess ? await success(_value!) : error(_error!);

    public async Task<T> MatchAsync<T>(
        Func<TValue, Task<T>> success,
        Func<Error, Task<T>> error
    ) => _isSuccess ? await success(_value!) : await error(_error!);
    
    public void Match(
        Action<TValue> success,
        Action<Error> error
    )
    {
        if (_isSuccess) success(_value!);
        else error(_error!);
    }

    public async Task MatchAsync(
        Func<TValue, Task> success,
        Action<Error> error
    )
    {
        if (_isSuccess) await success(_value!);
        else error(_error!);
    }

    public async Task<Result<TValue>> OnSuccessAsync(Func<TValue, Task> func)
    {
        if (_isSuccess) await func(_value!);
        return this;
    }

    public Result<TValue> OnSuccess(Action<TValue> func)
    {
        if (_isSuccess) func(_value!);
        return this;
    }

    public async Task<Result<TValue>> OnFailureAsync(Func<Error, Task> func)
    {
        if (!_isSuccess) await func(_error!);
        return this;
    }

    public Result<TValue> OnFailure(Action<Error> func)
    {
        if (!_isSuccess) func(_error!);
        return this;
    }

    public TValue OrElse(TValue defaultValue)
        => _isSuccess ? _value! : defaultValue;

    public Result<T> Map<T>(Func<TValue, T> mapper)
        => _isSuccess ? mapper(_value!) : _error!;

    public async Task<Result<T>> MapAsync<T>(Func<TValue, Task<T>> mapper)
        => _isSuccess ? await mapper(_value!) : _error!;

    public Result<T> Map<T>(Func<TValue, Result<T>> mapper)
        => _isSuccess
            ? mapper(_value!).Match<Result<T>>(success => success, err => err)
            : _error!;

    public async Task<Result<T>> MapAsync<T>(Func<TValue, Task<Result<T>>> mapper)
        => _isSuccess
            ? (await mapper(_value!)).Match<Result<T>>(success => success, err => err)
            : _error!;

    public static implicit operator Result<TValue>(TValue value)
        => new(value);

    public static implicit operator Result<TValue>(Error error)
        => new(error);
}