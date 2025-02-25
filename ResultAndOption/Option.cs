namespace ResultAndOption
{
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

    public abstract class Option<T>
    {
        public static implicit operator Option<T>(DelayedSome<T> some) => new ConcreteSome<T>(some.Value);

        public static implicit operator Option<T>(DelayedNone _) => new ConcreteNone<T>();

        public abstract bool IsSome();

        public abstract bool IsSomeAnd(Func<T, bool> f);

        public abstract bool IsNone();

        public abstract bool IsNoneOr(Func<T, bool> f);

        public abstract Option<U> Map<U>(Func<T, U> f);

        public abstract U MapOr<U>(U def, Func<T, U> f);

        public abstract U MapOrElse<U>(Func<U> def, Func<T, U> f);

        public abstract Option<T> Inspect(Action<T> f);

        public abstract T Expect(string message);

        public abstract T Unwrap();

        public abstract T UnwrapOr(T def);

        public abstract T UnwrapOrElse(Func<T> def);

        public abstract Result<T, E> OkOr<E>(E err);

        public abstract Result<T, E> OkOrElse<E>(Func<E> err);

        public abstract Option<U> And<U>(Option<U> other);

        public abstract Option<U> AndThen<U>(Func<T, Option<U>> f);

        public abstract Option<T> Filter(Func<T, bool> f);

        public abstract Option<T> Or(Option<T> other);

        public abstract Option<T> OrElse(Func<Option<T>> other);

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
}
