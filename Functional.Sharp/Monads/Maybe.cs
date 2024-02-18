using JetBrains.Annotations;

namespace Functional.Sharp.Monads;

[PublicAPI]
public readonly struct Maybe<T>
{
    private readonly T? _value;
    public readonly bool HasValue;

    private Maybe(T? value, bool hasValue)
        => (_value, HasValue) = (value, hasValue);

    public static Maybe<T> Of(T value)
        => new (value, value is not null);

    public Maybe<TResult> Map<TResult>(Func<T, TResult> mapper)
        => _value is null ? Maybe<TResult>.None() : Maybe<TResult>.Of(mapper(_value));

    public T OrElse(T defaultValue)
        => HasValue ? _value! : defaultValue;

    public T UnwrapOrThrow()
        => HasValue ? _value! : throw new NullReferenceException("Maybe did not contain a value to unwrap.");
    
    public static Maybe<T> None()
        => new (default, false);

    public static Maybe<T> Parse<TNullable>(TNullable? value)
        where TNullable : struct
        => value.HasValue ? Of((T)(object)value.Value) : None();

    public override bool Equals(object? obj)
        => obj switch
        {
            Maybe<T> other => Equals(_value, other._value),
            _ => false
        };
    
    public bool Equals(T? val)
        => _value is not null && (val?.Equals(_value) ?? false);

    public override int GetHashCode()
        => _value?.GetHashCode() ?? 0;

    public static bool operator ==(Maybe<T> first, Maybe<T> second)
        => first.Equals(second);

    public static bool operator !=(Maybe<T> first, Maybe<T> second) => !(first == second);
    
    public static implicit operator Maybe<T>(T? value) 
        => value is null ? Of(value!) : None();
}