namespace ResultAndOption
{
    public static class Result
    {
        public static DelayedOk<T> Ok<T>(T value) => new(value);

        public static DelayedErr<E> Err<E>(E error) => new(error);
    }

    public sealed class DelayedOk<T>
    {
        internal readonly T Value;

        internal DelayedOk(T value)
        {
            Value = value;
        }
    }

    public sealed class DelayedErr<E>
    {
        internal readonly E Error;

        internal DelayedErr(E error)
        {
            Error = error;
        }
    }

    /// <summary>
    /// Represents the result of an operation that can either be a success (Ok) or a failure (Err).
    /// </summary>
    /// <typeparam name="T">The type of the value that represents a successful result.</typeparam>
    /// <typeparam name="E">The type of the value that represents an error result.</typeparam>
    public abstract class Result<T, E>
    {
        public static implicit operator Result<T, E>(DelayedOk<T> ok) => new ConcreteOk<T, E>(ok.Value);

        public static implicit operator Result<T, E>(DelayedErr<E> err) => new ConcreteErr<T, E>(err.Error);

        /// <summary>
        /// Returns true if the result is Ok.
        /// </summary>
        public abstract bool IsOk();

        /// <summary>
        /// Returns true if the result is Err.
        /// </summary>
        public abstract bool IsErr();

        /// <summary>
        /// Returns true if the result is Ok and the provided function returns true.
        /// </summary>
        /// <param name="f">A function to test the Ok value.</param>
        public abstract bool IsOkAnd(Func<T, bool> f);

        /// <summary>
        /// Returns true if the result is Err and the provided function returns true.
        /// </summary>
        /// <param name="f">A function to test the Err value.</param>
        public abstract bool IsErrAnd(Func<E, bool> f);

        /// <summary>
        /// Returns the Ok value as an Option if the result is Ok, otherwise returns None.
        /// </summary>
        public abstract Option<T> Ok();

        /// <summary>
        /// Returns the Err value as an Option if the result is Err, otherwise returns None.
        /// </summary>
        public abstract Option<E> Err();

        /// <summary>
        /// Maps a Result<T, E> to Result<U, E> by applying a function to the Ok value.
        /// </summary>
        /// <param name="ok">A function to apply to the Ok value.</param>
        public abstract Result<U, E> Map<U>(Func<T, U> ok);

        /// <summary>
        /// Maps a Result<T, E> to U by applying a function to the Ok value or returns a default value if the result is Err.
        /// </summary>
        /// <param name="def">A default value to return if the result is Err.</param>
        /// <param name="f">A function to apply to the Ok value.</param>
        public abstract U MapOr<U>(U def, Func<T, U> f);

        /// <summary>
        /// Maps a Result<T, E> to U by applying a function to the Ok value or a fallback function to the Err value.
        /// </summary>
        /// <param name="def">A fallback function to apply to the Err value.</param>
        /// <param name="f">A function to apply to the Ok value.</param>
        public abstract U MapOrElse<U>(Func<E, U> def, Func<T, U> f);

        /// <summary>
        /// Maps a Result<T, E> to Result<T, F> by applying a function to the Err value.
        /// </summary>
        /// <param name="f">A function to apply to the Err value.</param>
        public abstract Result<T, F> MapErr<F>(Func<E, F> f);

        /// <summary>
        /// Calls a function with the Ok value if the result is Ok.
        /// </summary>
        /// <param name="f">A function to call with the Ok value.</param>
        public abstract Result<T, E> Inspect(Action<T> f);

        /// <summary>
        /// Calls a function with the Err value if the result is Err.
        /// </summary>
        /// <param name="f">A function to call with the Err value.</param>
        public abstract Result<T, E> InspectErr(Action<E> f);

        /// <summary>
        /// Returns the Ok value or throws an exception with the provided message if the result is Err.
        /// </summary>
        /// <param name="message">A message to include in the exception if the result is Err.</param>
        public abstract T Expect(string message);

        /// <summary>
        /// Returns the Ok value or throws an exception if the result is Err.
        /// </summary>
        public abstract T Unwrap();

        /// <summary>
        /// Returns the Err value or throws an exception with the provided message if the result is Ok.
        /// </summary>
        /// <param name="message">A message to include in the exception if the result is Ok.</param>
        public abstract E ExpectErr(string message);

        /// <summary>
        /// Returns the Err value or throws an exception if the result is Ok.
        /// </summary>
        public abstract E UnwrapErr();

        /// <summary>
        /// Returns the provided result if the result is Ok, otherwise returns the Err value.
        /// </summary>
        /// <param name="other">A result to return if the result is Ok.</param>
        public abstract Result<U, E> And<U>(Result<U, E> other);

        /// <summary>
        /// Calls a function with the Ok value and returns the result if the result is Ok, otherwise returns the Err value.
        /// </summary>
        /// <param name="f">A function to call with the Ok value.</param>
        public abstract Result<U, E> AndThen<U>(Func<T, Result<U, E>> f);

        /// <summary>
        /// Returns the provided result if the result is Err, otherwise returns the Ok value.
        /// </summary>
        /// <param name="other">A result to return if the result is Err.</param>
        public abstract Result<T, F> Or<F>(Result<T, F> other);

        /// <summary>
        /// Calls a function with the Err value and returns the result if the result is Err, otherwise returns the Ok value.
        /// </summary>
        /// <param name="f">A function to call with the Err value.</param>
        public abstract Result<T, F> OrElse<F>(Func<E, Result<T, F>> f);

        /// <summary>
        /// Returns the Ok value or a default value if the result is Err.
        /// </summary>
        /// <param name="def">A default value to return if the result is Err.</param>
        public abstract T UnwrapOr(T def);

        /// <summary>
        /// Returns the Ok value or calls a function with the Err value and returns the result if the result is Err.
        /// </summary>
        /// <param name="f">A function to call with the Err value.</param>
        public abstract T UnwrapOrElse(Func<E, T> f);
    }

    public sealed class ConcreteOk<T, E> : Result<T, E>
    {
        private readonly T _value;

        internal ConcreteOk(T value)
        {
            _value = value;
        }

        public override bool IsOk() => true;

        public override bool IsErr() => false;

        public override bool IsOkAnd(Func<T, bool> f) => f(_value);

        public override bool IsErrAnd(Func<E, bool> f) => false;

        public override Option<T> Ok() => Option.Some(_value);

        public override Option<E> Err() => Option.None();

        public override Result<U, E> Map<U>(Func<T, U> f) => Result.Ok(f(_value));

        public override U MapOr<U>(U def, Func<T, U> f) => f(_value);

        public override U MapOrElse<U>(Func<E, U> def, Func<T, U> f) => f(_value);

        public override Result<T, F> MapErr<F>(Func<E, F> f) => Result.Ok(_value);

        public override Result<T, E> Inspect(Action<T> f)
        {
            f(_value);

            return this;
        }

        public override Result<T, E> InspectErr(Action<E> f) => this;

        public override T Expect(string message) => _value;

        public override T Unwrap() => _value;

        public override E ExpectErr(string message) => throw new Exception(message);

        public override E UnwrapErr() => throw new Exception("UnwrapErr on Ok");

        public override Result<U, E> And<U>(Result<U, E> other) => other;

        public override Result<U, E> AndThen<U>(Func<T, Result<U, E>> f) => f(_value);

        public override Result<T, F> Or<F>(Result<T, F> other) => Result.Ok(_value);

        public override Result<T, F> OrElse<F>(Func<E, Result<T, F>> f) => Result.Ok(_value);

        public override T UnwrapOr(T def) => _value;

        public override T UnwrapOrElse(Func<E, T> f) => _value;
    }

    public sealed class ConcreteErr<T, E> : Result<T, E>
    {
        private readonly E _error;

        internal ConcreteErr(E error)
        {
            _error = error;
        }

        public override bool IsOk() => false;

        public override bool IsErr() => true;

        public override bool IsOkAnd(Func<T, bool> f) => false;

        public override bool IsErrAnd(Func<E, bool> f) => f(_error);

        public override Option<T> Ok() => Option.None();

        public override Option<E> Err() => Option.Some(_error);

        public override Result<U, E> Map<U>(Func<T, U> f) => Result.Err(_error);

        public override U MapOr<U>(U def, Func<T, U> f) => def;

        public override U MapOrElse<U>(Func<E, U> def, Func<T, U> f) => def(_error);

        public override Result<T, F> MapErr<F>(Func<E, F> f) => Result.Err(f(_error));

        public override Result<T, E> Inspect(Action<T> f) => this;

        public override Result<T, E> InspectErr(Action<E> f)
        {
            f(_error);

            return this;
        }

        public override T Expect(string message) => throw new Exception(message);

        public override T Unwrap() => throw new Exception("Unwrap on Err");

        public override E ExpectErr(string message) => _error;

        public override E UnwrapErr() => _error;

        public override Result<U, E> And<U>(Result<U, E> other) => Result.Err(_error);

        public override Result<U, E> AndThen<U>(Func<T, Result<U, E>> f) => Result.Err(_error);

        public override Result<T, F> Or<F>(Result<T, F> other) => other;

        public override Result<T, F> OrElse<F>(Func<E, Result<T, F>> f) => f(_error);

        public override T UnwrapOr(T def) => def;

        public override T UnwrapOrElse(Func<E, T> f) => f(_error);
    }
}
