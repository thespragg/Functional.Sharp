using Functional.Sharp.Errors;
using JetBrains.Annotations;

namespace Functional.Sharp.Monads;

[PublicAPI]
public interface IResult<TSelf> where TSelf : IResult<TSelf>
{
    static abstract TSelf Failure(Error error);
}
