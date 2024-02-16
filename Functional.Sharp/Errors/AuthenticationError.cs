using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record AuthenticationError(string Message) : Error(Message);