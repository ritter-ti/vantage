using System;
using FluentAssertions;
using Goal.Infra.Crosscutting.Tests.Mocks;
using Xunit;

namespace Goal.Infra.Crosscutting.Tests.Ensuring;

public class Ensure_NotEqual
{
    [Fact]
    public void ThrowsExceptionGivenEqualsPrimitives()
    {
        int a = 1;
        int b = 1;

        Action act = () => Ensure.NotEqual(a, b);
        act.Should().Throw<Exception>().And.Message.Should().Be(Messages.BothValuesMustNotBeEqual);
    }

    [Fact]
    public void ThrowsExceptionGivenEqualsNonPrimitives()
    {
        var a = new TestObject1();
        TestObject1 b = a;

        Action act = () => Ensure.NotEqual(a, b);
        act.Should().Throw<Exception>().And.Message.Should().Be(Messages.BothValuesMustNotBeEqual);
    }

    [Fact]
    public void ThrowsExceptionGivenLeftNull()
    {
        var b = new TestObject1();

        Action act = () => Ensure.NotEqual(null, b);
        act.Should().Throw<Exception>().And.Message.Should().Be(Messages.BothValuesMustNotBeEqual);
    }

    [Fact]
    public void ThrowsExceptionGivenRightNull()
    {
        var a = new TestObject1();

        Action act = () => Ensure.NotEqual(a, null);
        act.Should().Throw<Exception>().And.Message.Should().Be(Messages.BothValuesMustNotBeEqual);
    }

    [Fact]
    public void EnsureGivenNotEqualsPrimitives()
    {
        int a = 1;
        int b = 2;

        Action act = () => Ensure.NotEqual(a, b);
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void EnsureGivenNotEqualsNonPrimitives()
    {
        var a = new TestObject1();
        var b = new TestObject1();

        Action act = () => Ensure.NotEqual(a, b);
        act.Should().NotThrow<Exception>();
    }
}
