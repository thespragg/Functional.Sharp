using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record NetworkError(string Message) : Error(Message);