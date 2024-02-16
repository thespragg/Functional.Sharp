using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record ValidationError(string Message) : Error(Message);