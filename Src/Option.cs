using Results;

namespace Options;

public static class Option
{
    public static DelayedSome<T> Some<T>(T value) => new(value);

    public static DelayedNone None() => new();
}

public sealed class DelayedSome<T>
{
    internal readonly T Value;
    internal DelayedSome(T value)
    {
        Value = value;
    }
}

public sealed class DelayedNone
{
    internal DelayedNone() { }
}

/// <summary>
/// Represents an optional value. An Option can either be Some (containing a value) or None (containing no value).
/// </summary>
/// <typeparam name="T">The type of the value that the Option may contain.</typeparam>
public abstract class Option<T>
{
    public static implicit operator Option<T>(DelayedSome<T> some) => new ConcreteSome<T>(some.Value);

    public static implicit operator Option<T>(DelayedNone _) => new ConcreteNone<T>();

    /// <summary>
    /// Returns true if the option is Some.
    /// </summary>
    public abstract bool IsSome();

    /// <summary>
    /// Returns true if the option is Some and the provided function returns true.
    /// </summary>
    /// <param name="f">A function to test the Some value.</param>
    public abstract bool IsSomeAnd(Func<T, bool> f);

    /// <summary>
    /// Returns true if the option is None.
    /// </summary>
    public abstract bool IsNone();

    /// <summary>
    /// Returns true if the option is None or the provided function returns true.
    /// </summary>
    /// <param name="f">A function to test the None value.</param>
    public abstract bool IsNoneOr(Func<T, bool> f);

    /// <summary>
    /// Maps an Option<T> to Option<U> by applying a function to the Some value.
    /// </summary>
    /// <param name="f">A function to apply to the Some value.</param>
    public abstract Option<U> Map<U>(Func<T, U> f);

    /// <summary>
    /// Maps an Option<T> to U by applying a function to the Some value or returns a default value if the option is None.
    /// </summary>
    /// <param name="def">A default value to return if the option is None.</param>
    /// <param name="f">A function to apply to the Some value.</param>
    public abstract U MapOr<U>(U def, Func<T, U> f);

    /// <summary>
    /// Maps an Option<T> to U by applying a function to the Some value or a fallback function to the None value.
    /// </summary>
    /// <param name="def">A fallback function to apply to the None value.</param>
    /// <param name="f">A function to apply to the Some value.</param>
    public abstract U MapOrElse<U>(Func<U> def, Func<T, U> f);

    /// <summary>
    /// Calls a function with the Some value if the option is Some. Returns the option.
    /// </summary>
    /// <param name="f">A function to call with the Some value.</param>
    public abstract Option<T> Inspect(Action<T> f);

    /// <summary>
    /// Returns the Some value or throws an exception with the provided message if the option is None.
    /// </summary>
    /// <param name="message">A message to include in the exception if the option is None.</param>
    public abstract T Expect(string message);

    /// <summary>
    /// Returns the Some value or throws an exception if the option is None.
    /// </summary>
    public abstract T Unwrap();

    /// <summary>
    /// Returns the Some value or a default value if the option is None.
    /// </summary>
    /// <param name="def">A default value to return if the option is None.</param>
    public abstract T UnwrapOr(T def);

    /// <summary>
    /// Returns the Some value or calls a function to get a default value if the option is None.
    /// </summary>
    /// <param name="def">A function to call to get a default value if the option is None.</param>
    public abstract T UnwrapOrElse(Func<T> def);

    /// <summary>
    /// Returns a Result containing the Some value or a Result containing the provided error if the option is None.
    /// </summary>
    /// <param name="err">An error to include in the Result if the option is None.</param>
    public abstract Result<T, E> OkOr<E>(E err);

    /// <summary>
    /// Returns a Result containing the Some value or a Result containing an error from a function if the option is None.
    /// </summary>
    /// <param name="err">A function to call to get an error if the option is None.</param>
    public abstract Result<T, E> OkOrElse<E>(Func<E> err);

    /// <summary>
    /// Returns the provided option if the option is Some, otherwise returns None.
    /// </summary>
    /// <param name="other">An option to return if the option is Some.</param>
    public abstract Option<U> And<U>(Option<U> other);

    /// <summary>
    /// Calls a function with the Some value and returns the result if the option is Some, otherwise returns None.
    /// </summary>
    /// <param name="f">A function to call with the Some value.</param>
    public abstract Option<U> AndThen<U>(Func<T, Option<U>> f);

    /// <summary>
    /// Returns the option if the provided function returns true for the Some value, otherwise returns None.
    /// </summary>
    /// <param name="f">A function to test the Some value.</param>
    public abstract Option<T> Filter(Func<T, bool> f);

    /// <summary>
    /// Returns the option if it is Some, otherwise returns the provided option.
    /// </summary>
    /// <param name="other">An option to return if the option is None.</param>
    public abstract Option<T> Or(Option<T> other);

    /// <summary>
    /// Returns the option if it is Some, otherwise calls a function to get an option.
    /// </summary>
    /// <param name="other">A function to call to get an option if the option is None.</param>
    public abstract Option<T> OrElse(Func<Option<T>> other);

    /// <summary>
    /// Returns the option if exactly one of the options is Some, otherwise returns None.
    /// </summary>
    /// <param name="other">An option to compare with.</param>
    public abstract Option<T> Xor(Option<T> other);
}

public sealed class ConcreteSome<T> : Option<T>
{
    private readonly T _value;

    internal ConcreteSome(T value)
    {
        _value = value;
    }

    public override bool IsSome() => true;

    public override bool IsSomeAnd(Func<T, bool> f) => f(_value);

    public override bool IsNone() => false;

    public override bool IsNoneOr(Func<T, bool> f) => f(_value);

    public override Option<U> Map<U>(Func<T, U> f) => Option.Some(f(_value));

    public override U MapOr<U>(U def, Func<T, U> f) => f(_value);

    public override U MapOrElse<U>(Func<U> def, Func<T, U> f) => f(_value);

    public override Option<T> Inspect(Action<T> f)
    {
        f(_value);

        return this;
    }

    public override T Expect(string message) => _value;

    public override T Unwrap() => _value;

    public override T UnwrapOr(T def) => _value;

    public override T UnwrapOrElse(Func<T> def) => _value;

    public override Result<T, E> OkOr<E>(E err) => Result.Ok(_value);

    public override Result<T, E> OkOrElse<E>(Func<E> err) => Result.Ok(_value);

    public override Option<U> And<U>(Option<U> other) => other;

    public override Option<U> AndThen<U>(Func<T, Option<U>> f) => f(_value);

    public override Option<T> Filter(Func<T, bool> f) => f(_value) ? this : Option.None();

    public override Option<T> Or(Option<T> other) => this;

    public override Option<T> OrElse(Func<Option<T>> other) => this;

    public override Option<T> Xor(Option<T> other) => other.IsNone() ? this : Option.None();
}

public sealed class ConcreteNone<T> : Option<T>
{
    public override bool IsSome() => false;

    public override bool IsSomeAnd(Func<T, bool> f) => false;

    public override bool IsNone() => true;

    public override bool IsNoneOr(Func<T, bool> f) => true;

    public override Option<U> Map<U>(Func<T, U> f) => Option.None();

    public override U MapOr<U>(U def, Func<T, U> f) => def;

    public override U MapOrElse<U>(Func<U> def, Func<T, U> f) => def();

    public override Option<T> Inspect(Action<T> f) => this;

    public override T Expect(string message) => throw new InvalidOperationException(message);

    public override T Unwrap() => throw new InvalidOperationException("Called Unwrap on a None value");

    public override T UnwrapOr(T def) => def;

    public override T UnwrapOrElse(Func<T> def) => def();

    public override Result<T, E> OkOr<E>(E err) => Result.Err(err);

    public override Result<T, E> OkOrElse<E>(Func<E> err) => Result.Err(err());

    public override Option<U> And<U>(Option<U> other) => Option.None();

    public override Option<U> AndThen<U>(Func<T, Option<U>> f) => Option.None();

    public override Option<T> Filter(Func<T, bool> f) => this;

    public override Option<T> Or(Option<T> other) => other;

    public override Option<T> OrElse(Func<Option<T>> other) => other();

    public override Option<T> Xor(Option<T> other) => other;
}
