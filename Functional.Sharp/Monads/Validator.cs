using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional.Sharp.Monads;

public static class Validator
{
    public static Validator<T> For<T>() => new();
}

public class Validator<T>
{
    private IEnumerable<(Func<T, bool>, string)> Predicates { get; }

    internal Validator()
        => Predicates = Enumerable.Empty<(Func<T, bool>, string)>();

    private Validator(
        IEnumerable<(Func<T, bool>, string)> predicates
    ) => Predicates = predicates;

    public Validator<T> AddRule(
        Func<T, bool> predicate,
        string errorMessage
    ) => new(
        Predicates.Concat(
            new[] { (predicate, errorMessage) }
        )
    );

    public IEnumerable<string> Validate(T value)
        => Predicates
            .Select(x => x.Item1(value) ? null : x.Item2)
            .Where(x => x is not null)
            .Cast<string>();
}