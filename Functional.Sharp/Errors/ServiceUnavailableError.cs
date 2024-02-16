using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record ServiceUnavailableError(string Message) : Error(Message);