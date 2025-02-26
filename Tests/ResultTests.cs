using Options;
using Results;

namespace ResultTests;

internal static class ResultFactory
{
    public static Result<int, int> Ok(int value = 1) => Result.Ok(value);

    public static Result<int, int> Err(int error = 1) => Result.Err(error);
}

[TestClass]
public sealed class IsOkTests
{
    [TestMethod]
    public void Ok_IsOk()
    {
        var result = ResultFactory.Ok();

        Assert.IsTrue(result.IsOk());
    }

    [TestMethod]
    public void Err_Not_IsOk()
    {
        var result = ResultFactory.Err();

        Assert.IsFalse(result.IsOk());
    }
}

[TestClass]
public sealed class IsErrTests
{
    [TestMethod]
    public void Err_IsErr()
    {
        var result = ResultFactory.Err();

        Assert.IsTrue(result.IsErr());
    }

    [TestMethod]
    public void Ok_Not_IsErr()
    {
        var result = ResultFactory.Ok();

        Assert.IsFalse(result.IsErr());
    }
}

[TestClass]
public sealed class IsOkAndTests
{
    [TestMethod]
    public void Ok_IsOkAndTrue()
    {
        var result = ResultFactory.Ok();

        Assert.IsTrue(result.IsOkAnd(x => x == 1));
    }

    [TestMethod]
    public void Ok_IsOkAndFalse()
    {
        var result = ResultFactory.Ok();

        Assert.IsFalse(result.IsOkAnd(x => x == 2));
    }

    [TestMethod]
    public void Err_Not_IsOkAndTrue()
    {
        var result = ResultFactory.Err();

        Assert.IsFalse(result.IsOkAnd(x => x == 1));
    }

    [TestMethod]
    public void Err_Not_IsOkAndFalse()
    {
        var result = ResultFactory.Err();

        Assert.IsFalse(result.IsOkAnd(x => x == 2));
    }
}

[TestClass]
public sealed class IsErrAndTests
{
    [TestMethod]
    public void Err_IsErrAndTrue()
    {
        var result = ResultFactory.Err();

        Assert.IsTrue(result.IsErrAnd(x => x == 1));
    }
    [TestMethod]
    public void Err_IsErrAndFalse()
    {
        var result = ResultFactory.Err();

        Assert.IsFalse(result.IsErrAnd(x => x == 2));
    }
    [TestMethod]
    public void Ok_Not_IsErrAndTrue()
    {
        var result = ResultFactory.Ok();

        Assert.IsFalse(result.IsErrAnd(x => x == 1));
    }
    [TestMethod]
    public void Ok_Not_IsErrAndFalse()
    {
        var result = ResultFactory.Ok();

        Assert.IsFalse(result.IsErrAnd(x => x == 2));
    }
}

[TestClass]
public sealed class OptionOkTests
{
    [TestMethod]
    public void Ok_Some()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Option.Some(1), result.Ok());
    }

    [TestMethod]
    public void Err_None()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Option.None(), result.Ok());
    }
}

[TestClass]
public sealed class OptionErrTests
{
    [TestMethod]
    public void Err_Some()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Option.Some(1), result.Err());
    }
    [TestMethod]
    public void Ok_None()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Option.None(), result.Err());
    }
}

[TestClass]
public sealed class MapTests
{
    [TestMethod]
    public void Ok_Map()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Result.Ok(2), result.Map(x => x + 1));
    }
    [TestMethod]
    public void Err_Map()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Err(1), result.Map(x => x + 1));
    }
}

[TestClass]
public sealed class MapOrTests
{
    [TestMethod]
    public void Ok_MapOr()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(2, result.MapOr(0, x => x + 1));
    }
    [TestMethod]
    public void Err_MapOr()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(0, result.MapOr(0, x => x + 1));
    }
}

[TestClass]
public sealed class MapOrElseTests
{
    [TestMethod]
    public void Ok_MapOrElse()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(2, result.MapOrElse(_ => 0, x => x + 1));
    }
    [TestMethod]
    public void Err_MapOrElse()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(0, result.MapOrElse(_ => 0, x => x + 1));
    }
}

[TestClass]
public sealed class MapErrTests
{
    [TestMethod]
    public void Ok_MapErr()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Result.Ok(1), result.MapErr(x => x + 1));
    }
    [TestMethod]
    public void Err_MapErr()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Err(2), result.MapErr(x => x + 1));
    }
}

[TestClass]
public sealed class InspectTests
{
    [TestMethod]
    public void Ok_Inspect()
    {
        var result = ResultFactory.Ok();

        result.Inspect(x => Assert.AreEqual(1, x));
    }
    [TestMethod]
    public void Err_Inspect()
    {
        var result = ResultFactory.Err();

        result.Inspect(x => throw new Exception("This should not throw"));
    }
}

[TestClass]
public sealed class InspectErrTests
{
    [TestMethod]
    public void Ok_InspectErr()
    {
        var result = ResultFactory.Ok();

        result.InspectErr(x => throw new Exception("This should not throw"));
    }
    [TestMethod]
    public void Err_InspectErr()
    {
        var result = ResultFactory.Err();

        result.InspectErr(x => Assert.AreEqual(1, x));
    }
}

[TestClass]
public sealed class ExpectTests
{
    [TestMethod]
    public void Ok_Expect()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(1, result.Expect("This should not throw"));
    }
    [TestMethod]
    public void Err_Expect()
    {
        var result = ResultFactory.Err();

        Assert.ThrowsException<Exception>(() => result.Expect("This should throw"));
    }
}

[TestClass]
public sealed class ExpectErrTests
{
    [TestMethod]
    public void Ok_ExpectErr()
    {
        var result = ResultFactory.Ok();

        Assert.ThrowsException<Exception>(() => result.ExpectErr("This should throw"));
    }
    [TestMethod]
    public void Err_ExpectErr()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(1, result.ExpectErr("This should not throw"));
    }
}

[TestClass]
public sealed class UnwrapTests
{
    [TestMethod]
    public void Ok_Unwrap()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(1, result.Unwrap());
    }
    [TestMethod]
    public void Err_Unwrap()
    {
        var result = ResultFactory.Err();

        Assert.ThrowsException<Exception>(() => result.Unwrap());
    }
}

[TestClass]
public sealed class UnwrapErrTests
{
    [TestMethod]
    public void Ok_UnwrapErr()
    {
        var result = ResultFactory.Ok();

        Assert.ThrowsException<Exception>(() => result.UnwrapErr());
    }
    [TestMethod]
    public void Err_UnwrapErr()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(1, result.UnwrapErr());
    }
}

[TestClass]
public sealed class UnwrapOrTests
{
    [TestMethod]
    public void Ok_UnwrapOr()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(1, result.UnwrapOr(0));
    }
    [TestMethod]
    public void Err_UnwrapOr()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(0, result.UnwrapOr(0));
    }
}

[TestClass]
public sealed class UnwrapOrElseTests
{
    [TestMethod]
    public void Ok_UnwrapOrElse()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(1, result.UnwrapOrElse(_ => 0));
    }
    [TestMethod]
    public void Err_UnwrapOrElse()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(0, result.UnwrapOrElse(_ => 0));
    }
}

[TestClass]
public sealed class OrElseTests
{
    [TestMethod]
    public void Ok_OrElse()
    {

        var result = ResultFactory.Ok();
        Assert.AreEqual(Result.Ok(1), result.OrElse<int>(_ => Result.Ok(0)));
    }
    [TestMethod]
    public void Err_OrElse()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Ok(0), result.OrElse<int>(_ => Result.Ok(0)));
    }
}

[TestClass]
public sealed class AndTests
{
    [TestMethod]
    public void Ok_And()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Result.Ok(0), result.And<int>(Result.Ok(0)));
    }
    [TestMethod]
    public void Err_And()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Err(1), result.And<int>(Result.Ok(0)));
    }
}

[TestClass]

public sealed class AndThenTests
{
    [TestMethod]
    public void Ok_AndThen()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Result.Ok(0), result.AndThen<int>(_ => Result.Ok(0)));
    }
    [TestMethod]
    public void Err_AndThen()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Err(1), result.AndThen<int>(_ => Result.Ok(0)));
    }
}

[TestClass]
public sealed class OrTests
{
    [TestMethod]
    public void Ok_Or()
    {
        var result = ResultFactory.Ok();

        Assert.AreEqual(Result.Ok(1), result.Or<int>(Result.Ok(0)));
    }
    [TestMethod]
    public void Err_Or()
    {
        var result = ResultFactory.Err();

        Assert.AreEqual(Result.Ok(0), result.Or<int>(Result.Ok(0)));
    }
}
