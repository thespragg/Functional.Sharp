using JetBrains.Annotations;

namespace Functional.Sharp.Errors;

[PublicAPI]
public record Error(string Message);