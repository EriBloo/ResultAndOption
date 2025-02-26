using Options;
using Results;

namespace OptionsTests;

internal static class OptionFactory
{
    public static Option<int> Some(int value = 1) => Option.Some(value);

    public static Option<int> None() => Option.None();
}

[TestClass]
public sealed class IsSomeTests
{
    [TestMethod]
    public void Some_IsSome()
    {
        var option = OptionFactory.Some();

        Assert.IsTrue(option.IsSome());
    }
    [TestMethod]
    public void None_Not_IsSome()
    {
        var option = OptionFactory.None();

        Assert.IsFalse(option.IsSome());
    }
}

[TestClass]
public sealed class IsSomeAndTests
{
    [TestMethod]
    public void Some_IsSomeAndTrue()
    {
        var option = OptionFactory.Some();

        Assert.IsTrue(option.IsSomeAnd(x => x == 1));
    }
    [TestMethod]
    public void Some_Not_IsSomeAndFalse()
    {
        var option = OptionFactory.Some();

        Assert.IsFalse(option.IsSomeAnd(x => x == 2));
    }
    [TestMethod]
    public void None_Not_IsSomeAndFalse()
    {
        var option = OptionFactory.None();

        Assert.IsFalse(option.IsSomeAnd(x => x == 1));
    }
}

[TestClass]
public sealed class IsNoneTests
{
    [TestMethod]
    public void None_IsNone()
    {
        var option = OptionFactory.None();

        Assert.IsTrue(option.IsNone());
    }
    [TestMethod]
    public void Some_Not_IsNone()
    {
        var option = OptionFactory.Some();

        Assert.IsFalse(option.IsNone());
    }
}

[TestClass]
public sealed class IsNoneOrTests
{
    [TestMethod]
    public void None_IsNoneOrTrue()
    {
        var option = OptionFactory.None();

        Assert.IsTrue(option.IsNoneOr(x => x == 1));
    }
    [TestMethod]
    public void None_IsNoneOrFalse()
    {
        var option = OptionFactory.None();

        Assert.IsTrue(option.IsNoneOr(x => x == 2));
    }
    [TestMethod]
    public void Some_Not_IsNoneOrTrue()
    {
        var option = OptionFactory.Some();

        Assert.IsTrue(option.IsNoneOr(x => x == 1));
    }
    [TestMethod]
    public void Some_Not_IsNoneOrFalse()
    {
        var option = OptionFactory.Some();

        Assert.IsFalse(option.IsNoneOr(x => x == 2));
    }
}

[TestClass]
public sealed class MapTests
{
    [TestMethod]
    public void Some_Map()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(Option.Some(2), option.Map(x => x + 1));
    }
    [TestMethod]
    public void None_Not_Map()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.Map(x => x + 1));
    }
}

[TestClass]
public sealed class MapOrTests
{
    [TestMethod]
    public void Some_MapOr()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(2, option.MapOr(0, x => x + 1));
    }
    [TestMethod]
    public void None_MapOr()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(0, option.MapOr(0, x => x + 1));
    }
}

[TestClass]
public sealed class MapOrElseTests
{
    [TestMethod]
    public void Some_MapOrElse()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(2, option.MapOrElse(() => 0, x => x + 1));
    }
    [TestMethod]
    public void None_MapOrElse()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(0, option.MapOrElse(() => 0, x => x + 1));
    }
}

[TestClass]
public sealed class InspectTests
{
    [TestMethod]
    public void Some_Inspect()
    {
        var option = OptionFactory.Some();
        var result = 0;

        option.Inspect(x => result = x);

        Assert.AreEqual(1, result);
    }
    [TestMethod]
    public void None_Not_Inspect()
    {
        var option = OptionFactory.None();
        var result = 0;

        option.Inspect(x => result = x);

        Assert.AreEqual(0, result);
    }
}

[TestClass]
public sealed class AndTests
{
    [TestMethod]
    public void Some_AndSome()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(2), option.And(other));
    }
    [TestMethod]
    public void Some_AndNone()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.And(other));
    }
    [TestMethod]
    public void None_AndSome()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.None(), option.And(other));
    }
    [TestMethod]
    public void None_AndNone()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.And(other));
    }
}

[TestClass]
public sealed class AndThenTests
{
    [TestMethod]
    public void Some_AndThenSome()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(2), option.AndThen(x => other));
    }
    [TestMethod]
    public void Some_AndThenNone()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.AndThen(x => other));
    }
    [TestMethod]
    public void None_AndThenSome()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.None(), option.AndThen(x => other));
    }
    [TestMethod]
    public void None_AndThenNone()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.AndThen(x => other));
    }
}

[TestClass]
public sealed class FilterTests
{
    [TestMethod]
    public void Some_FilterTrue()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(Option.Some(1), option.Filter(x => x == 1));
    }
    [TestMethod]
    public void Some_FilterFalse()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(Option.None(), option.Filter(x => x == 2));
    }
    [TestMethod]
    public void None_Not_Filter()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.Filter(x => x == 1));
    }
}

[TestClass]
public sealed class OrTests
{
    [TestMethod]
    public void Some_OrSome()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(1), option.Or(other));
    }
    [TestMethod]
    public void Some_OrNone()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.Some(1), option.Or(other));
    }
    [TestMethod]
    public void None_OrSome()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(2), option.Or(other));
    }
    [TestMethod]
    public void None_OrNone()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.Or(other));
    }
}

[TestClass]
public sealed class OrElseTests
{
    [TestMethod]
    public void Some_OrElseSome()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(1), option.OrElse(() => other));
    }
    [TestMethod]
    public void Some_OrElseNone()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.Some(1), option.OrElse(() => other));
    }
    [TestMethod]
    public void None_OrElseSome()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.Some(2);

        Assert.AreEqual(Option.Some(2), option.OrElse(() => other));
    }
    [TestMethod]
    public void None_OrElseNone()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.None();

        Assert.AreEqual(Option.None(), option.OrElse(() => other));
    }
}

[TestClass]
public sealed class XorTests
{
    [TestMethod]
    public void Some_XorSome()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.Some(2);
        Assert.AreEqual(Option.None(), option.Xor(other));
    }
    [TestMethod]
    public void Some_XorNone()
    {
        var option = OptionFactory.Some();
        var other = OptionFactory.None();
        Assert.AreEqual(Option.Some(1), option.Xor(other));
    }
    [TestMethod]
    public void None_XorSome()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.Some(2);
        Assert.AreEqual(Option.Some(2), option.Xor(other));
    }
    [TestMethod]
    public void None_XorNone()
    {
        var option = OptionFactory.None();
        var other = OptionFactory.None();
        Assert.AreEqual(Option.None(), option.Xor(other));
    }
}

[TestClass]
public sealed class UnwrapTests
{
    [TestMethod]
    public void Some_Unwrap()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(1, option.Unwrap());
    }
    [TestMethod]
    public void None_Throws()
    {
        var option = OptionFactory.None();

        Assert.ThrowsException<InvalidOperationException>(() => option.Unwrap());
    }
}

[TestClass]
public sealed class ExpectTests
{
    [TestMethod]
    public void Some_Expect()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(1, option.Expect("panic"));
    }
    [TestMethod]
    public void None_Expect()
    {
        var option = OptionFactory.None();

        Assert.ThrowsException<InvalidOperationException>(() => option.Expect("panic"));
    }
}

[TestClass]
public sealed class UnwrapOrTests
{
    [TestMethod]
    public void Some_UnwrapOr()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(1, option.UnwrapOr(0));
    }
    [TestMethod]
    public void None_UnwrapOr()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(0, option.UnwrapOr(0));
    }
}

[TestClass]
public sealed class UnwrapOrElseTests
{
    [TestMethod]
    public void Some_UnwrapOrElse()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(1, option.UnwrapOrElse(() => 0));
    }
    [TestMethod]
    public void None_UnwrapOrElse()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(0, option.UnwrapOrElse(() => 0));
    }
}

[TestClass]
public sealed class OkOrTests
{
    [TestMethod]
    public void Some_OkOr()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(Result.Ok(1), option.OkOr(0));
    }

    [TestMethod]
    public void None_OkOr()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(Result.Err(0), option.OkOr(0));
    }
}

[TestClass]
public sealed class OkOrElseTests
{
    [TestMethod]
    public void Some_OkOrElse()
    {
        var option = OptionFactory.Some();

        Assert.AreEqual(Result.Ok(1), option.OkOrElse(() => 0));
    }
    [TestMethod]
    public void None_OkOrElse()
    {
        var option = OptionFactory.None();

        Assert.AreEqual(Result.Err(0), option.OkOrElse(() => 0));
    }
}