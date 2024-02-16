using Functional.Sharp.Errors;
using Functional.Sharp.Monads;

namespace Functional.Sharp.Helpers;

public static class Try
{
    private static readonly Func<Exception, Error> DefaultExceptionMapper = ex => new Error(ex.Message);

    private static Result<T> ExecuteCore<T>(
        Func<T> func,
        Func<Exception, Error>? exceptionMapper = null
    )
    {
        exceptionMapper ??= DefaultExceptionMapper;
        try
        {
            return Result<T>.Success(func());
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(exceptionMapper(ex));
        }
    }

    private static async Task<Result<TResult>> ExecuteCoreAsync<TResult>(
        Func<Task<TResult>> func,
        Func<Exception, Error>? exceptionMapper = null
    )
    {
        exceptionMapper ??= DefaultExceptionMapper;
        try
        {
            return Result<TResult>.Success(await func());
        }
        catch (Exception ex)
        {
            return Result<TResult>.Failure(exceptionMapper(ex));
        }
    }

    public static bool Execute(
        Action func,
        Func<Exception, Error>? exceptionMapper = null
    ) => ExecuteCore(() =>
    {
        func();
        return true;
    }, exceptionMapper).OrElse(false);

    public static Result<TResult> Execute<TResult>(
        Func<TResult> func,
        Func<Exception, Error>? exceptionMapper = null
    ) => ExecuteCore(func, exceptionMapper);

    public static async Task<Result<TResult>> ExecuteAsync<TResult>(
        Func<Task<TResult>> func,
        Func<Exception, Error>? exceptionMapper = null
    ) => await ExecuteCoreAsync(func, exceptionMapper);

    public static async Task<bool> ExecuteAsync(
        Func<Task> func,
        Func<Exception, Error>? exceptionMapper = null
    ) => (await ExecuteCoreAsync(async () =>
    {
        await func();
        return true;
    }, exceptionMapper)).OrElse(false);
}