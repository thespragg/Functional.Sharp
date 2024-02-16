using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record AuthorizationError(string Message) : Error(Message);