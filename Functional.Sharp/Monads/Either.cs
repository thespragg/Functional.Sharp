using JetBrains.Annotations;

namespace Functional.Sharp.Monads;

[PublicAPI]
public abstract class Either<TLeft, TRight>
{
    public abstract T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc);
    public abstract void Match(Action<TLeft> leftFunc, Action<TRight> rightFunc);
    public abstract Task<T> MatchAsync<T>(Func<TLeft, Task<T>> leftFunc, Func<TRight, Task<T>> rightFunc);
    public abstract Task<T> MatchAsync<T>(Func<TLeft, Task<T>> leftFunc, Func<TRight, T> rightFunc);
    public abstract Task<T> MatchAsync<T>(Func<TLeft, T> leftFunc, Func<TRight, Task<T>> rightFunc);
    public abstract Either<TResult, TRight> MapLeft<TResult>(Func<TLeft, TResult> mapper);
    public abstract Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> mapper);
    public static Either<TLeft, TRight> Left(TLeft left) => new LeftSide(left);
    public static Either<TLeft, TRight> Right(TRight right) => new RightSide(right);

    private class LeftSide(TLeft value) : Either<TLeft, TRight>
    {
        private TLeft Value { get; } = value;

        public override T Match<T>(
            Func<TLeft, T> leftFunc,
            Func<TRight, T> rightFunc
        ) => leftFunc(Value);

        public override void Match(
            Action<TLeft> leftFunc,
            Action<TRight> rightFunc
        ) => leftFunc(Value);

        public override async Task<T> MatchAsync<T>(
            Func<TLeft, Task<T>> leftFunc,
            Func<TRight, Task<T>> rightFunc
        ) => await leftFunc(Value);

        public override async Task<T> MatchAsync<T>(
            Func<TLeft, Task<T>> leftFunc,
            Func<TRight, T> rightFunc
        ) => await leftFunc(Value);

        public override Task<T> MatchAsync<T>(
            Func<TLeft, T> leftFunc,
            Func<TRight, Task<T>> rightFunc
        ) => Task.FromResult(leftFunc(Value));

        public override Either<TResult, TRight> MapLeft<TResult>(Func<TLeft, TResult> mapper)
            => Either<TResult, TRight>.Left(mapper(Value));

        public override Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> mapper)
            => Either<TLeft, TResult>.Left(Value);
    }

    private class RightSide(TRight value) : Either<TLeft, TRight>
    {
        private TRight Value { get; } = value;

        public override T Match<T>(
            Func<TLeft, T> leftFunc,
            Func<TRight, T> rightFunc
        ) => rightFunc(Value);

        public override void Match(
            Action<TLeft> leftFunc,
            Action<TRight> rightFunc
        ) => rightFunc(Value);

        public override Task<T> MatchAsync<T>(
            Func<TLeft, Task<T>> leftFunc,
            Func<TRight, Task<T>> rightFunc
        ) => rightFunc(Value);

        public override Task<T> MatchAsync<T>(
            Func<TLeft, Task<T>> leftFunc,
            Func<TRight, T> rightFunc
        ) => Task.FromResult(rightFunc(Value));

        public override async Task<T> MatchAsync<T>(
            Func<TLeft, T> leftFunc,
            Func<TRight, Task<T>> rightFunc
        ) => await rightFunc(Value);

        public override Either<TResult, TRight> MapLeft<TResult>(Func<TLeft, TResult> mapper)
            => Either<TResult, TRight>.Right(Value);

        public override Either<TLeft, TResult> Map<TResult>(Func<TRight, TResult> mapper)
            => Either<TLeft, TResult>.Right(mapper(Value));
    }
}
