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

    public abstract class Result<T, E>
    {
        public static implicit operator Result<T, E>(DelayedOk<T> ok) => new Ok<T, E>(ok.Value);

        public static implicit operator Result<T, E>(DelayedErr<E> err) => new Err<T, E>(err.Error);

        public abstract bool IsOk();

        public abstract bool IsErr();

        public abstract bool IsOkAnd(Func<T, bool> f);

        public abstract bool IsErrAnd(Func<E, bool> f);

        public abstract Result<U, E> Map<U>(Func<T, U> ok);

        public abstract U MapOr<U>(U def, Func<T, U> f);

        public abstract U MapOrElse<U>(Func<E, U> def, Func<T, U> f);

        public abstract Result<T, F> MapErr<F>(Func<E, F> f);

        public abstract Result<T, E> Inspect(Action<T> f);

        public abstract Result<T, E> InspectErr(Action<E> f);

        public abstract T Expect(string message);

        public abstract T Unwrap();

        public abstract E ExpectErr(string message);

        public abstract E UnwrapErr();

        public abstract Result<U, E> And<U>(Result<U, E> other);

        public abstract Result<U, E> AndThen<U>(Func<T, Result<U, E>> f);

        public abstract Result<T, F> Or<F>(Result<T, F> other);

        public abstract Result<T, F> OrElse<F>(Func<E, Result<T, F>> f);

        public abstract T UnwrapOr(T def);

        public abstract T UnwrapOrElse(Func<E, T> f);
    }

    public sealed class Ok<T, E> : Result<T, E>
    {
        private readonly T _value;

        internal Ok(T value)
        {
            _value = value;
        }

        public override bool IsOk() => true;

        public override bool IsErr() => false;

        public override bool IsOkAnd(Func<T, bool> f) => IsOk() && f(_value);

        public override bool IsErrAnd(Func<E, bool> f) => false;

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

    public sealed class Err<T, E> : Result<T, E>
    {
        private readonly E _error;

        internal Err(E error)
        {
            _error = error;
        }

        public override bool IsOk() => false;

        public override bool IsErr() => true;

        public override bool IsOkAnd(Func<T, bool> f) => false;

        public override bool IsErrAnd(Func<E, bool> f) => IsErr() && f(_error);

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
