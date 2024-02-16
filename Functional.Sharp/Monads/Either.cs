using JetBrains.Annotations;

namespace Functional.Sharp.Monads;

[PublicAPI]
public abstract class Either<TLeft, TRight>
{
    public abstract T Match<T>(Func<TLeft, T> leftFunc, Func<TRight, T> rightFunc);
    public abstract void Match(Action<TLeft> leftFunc, Action<TRight> rightFunc);
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
    }
}