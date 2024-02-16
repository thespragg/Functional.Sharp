using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record PermissionError(string Message) : Error(Message);