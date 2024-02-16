using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record TimeoutError(string Message) : Error(Message);