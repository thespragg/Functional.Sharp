using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record DatabaseError(string Message) : Error(Message);