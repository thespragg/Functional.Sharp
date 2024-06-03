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
        => new(value, value is not null);

    public Maybe<TResult> Map<TResult>(Func<T, TResult> mapper)
        => _value is null
            ? Maybe<TResult>.None
            : mapper(_value);
    
    public async Task<Maybe<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> mapper)
        => _value is null
            ? Maybe<TResult>.None
            : await mapper(_value);

    public T OrElse(T defaultValue)
        => HasValue ? _value! : defaultValue;

    public TResult Match<TResult>(
        Func<T, TResult> hasValue,
        Func<TResult> hasNone
    ) => HasValue 
        ? hasValue(_value!) 
        : hasNone();
    
    public async Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> hasValue,
        Func<TResult> hasNone
    ) => HasValue 
        ? await hasValue(_value!) 
        : hasNone();

    public static Maybe<T> None
        => new(default, false);

    public static Maybe<T> From<TNullable>(TNullable? value)
        where TNullable : struct
        => value.HasValue ? Of((T)(object)value.Value) : None;

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
        => value is not null ? Of(value) : None;
}