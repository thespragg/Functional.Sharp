using JetBrains.Annotations;

namespace Functional.Sharp;

[PublicAPI]
public readonly struct Maybe<T>
{
    private readonly T _value;

    private Maybe(T val)
    {
        _value = val;
        HasValue = true;
    }

    public static Maybe<T> Parse(T? value)
        => value is not null ? Some(value) : None;

    public static Maybe<T> Parse<TNullable>(TNullable? value)
        where TNullable : struct
        => value.HasValue ? Some((T)(object)value.Value) : None;


    public static Maybe<T> Some(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        return new Maybe<T>(value);
    }

    public static Maybe<T> None => new();

    public T Value
    {
        get
        {
            if (!HasValue)
                throw new InvalidOperationException("Maybe does not have a value");

            return _value;
        }
    }

    public bool HasValue { get; }

    public static Maybe<TOut> FromMaybe<TIn, TOut>(
        Maybe<TIn> input,
        Func<TIn, TOut> converter)
        => input.HasValue ? Maybe<TOut>.Some(converter(input.Value)) : Maybe<TOut>.None;

    public static Maybe<TOut> FromMaybe<TIn, TOut>(
        Maybe<TIn> input,
        Func<TIn, Maybe<TOut>> converter)
    {
        var val = input.HasValue ? converter(input.Value) : default;
        return val.HasValue ? Maybe<TOut>.Some(val.Value) : Maybe<TOut>.None;
    }

    public T? GetValueOrDefault(T? defaultValue)
        => HasValue ? _value : defaultValue;

    public override bool Equals(object? obj)
        => obj switch
        {
            Maybe<T> other => Equals(_value, other._value),
            _ => false
        };


    public bool Equals(T? val)
        => HasValue && (val?.Equals(Value) ?? false);

    public override int GetHashCode()
        => _value?.GetHashCode() ?? 0;

    public static bool operator ==(Maybe<T> maybe, T value)
        => maybe.Value != null && maybe.HasValue && maybe.Value.Equals(value);

    public static bool operator !=(Maybe<T> maybe, T value)
        => maybe.Value != null && maybe.HasValue && !maybe.Value.Equals(value);

    public static bool operator ==(Maybe<T> first, Maybe<T> second)
        => first.Equals(second);

    public static bool operator !=(Maybe<T> first, Maybe<T> second) => !(first == second);
}