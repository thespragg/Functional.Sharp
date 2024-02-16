using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record NotFoundError(string Message) : Error(Message);