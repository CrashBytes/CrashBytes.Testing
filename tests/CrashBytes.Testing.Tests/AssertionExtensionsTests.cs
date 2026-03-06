using Xunit;

namespace CrashBytes.Testing.Tests;

public class AssertionExtensionsTests
{
    [Fact]
    public void ShouldBe_WhenEqual_DoesNotThrow()
    {
        42.ShouldBe(42);
        "hello".ShouldBe("hello");
    }

    [Fact]
    public void ShouldBe_WhenNotEqual_Throws()
    {
        var ex = Assert.Throws<AssertionException>(() => 42.ShouldBe(99));
        Assert.Contains("99", ex.Message);
        Assert.Contains("42", ex.Message);
    }

    [Fact]
    public void ShouldNotBe_WhenDifferent_DoesNotThrow()
    {
        42.ShouldNotBe(99);
    }

    [Fact]
    public void ShouldNotBe_WhenEqual_Throws()
    {
        Assert.Throws<AssertionException>(() => 42.ShouldNotBe(42));
    }

    [Fact]
    public void ShouldBeNull_WhenNull_DoesNotThrow()
    {
        string? value = null;
        value.ShouldBeNull();
    }

    [Fact]
    public void ShouldBeNull_WhenNotNull_Throws()
    {
        Assert.Throws<AssertionException>(() => "not null".ShouldBeNull());
    }

    [Fact]
    public void ShouldNotBeNull_WhenNotNull_DoesNotThrow()
    {
        "value".ShouldNotBeNull();
    }

    [Fact]
    public void ShouldNotBeNull_WhenNull_Throws()
    {
        string? value = null;
        Assert.Throws<AssertionException>(() => value.ShouldNotBeNull());
    }

    [Fact]
    public void ShouldContain_WhenPresent_DoesNotThrow()
    {
        var list = new List<int> { 1, 2, 3 };
        list.ShouldContain(2);
    }

    [Fact]
    public void ShouldContain_WhenAbsent_Throws()
    {
        var list = new List<int> { 1, 2, 3 };
        Assert.Throws<AssertionException>(() => list.ShouldContain(99));
    }

    [Fact]
    public void ShouldBeEmpty_WhenEmpty_DoesNotThrow()
    {
        var list = new List<int>();
        list.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldBeEmpty_WhenNotEmpty_Throws()
    {
        var list = new List<int> { 1 };
        Assert.Throws<AssertionException>(() => list.ShouldBeEmpty());
    }

    [Fact]
    public void ShouldNotBeEmpty_WhenNotEmpty_DoesNotThrow()
    {
        var list = new List<int> { 1 };
        list.ShouldNotBeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_WhenEmpty_Throws()
    {
        var list = new List<int>();
        Assert.Throws<AssertionException>(() => list.ShouldNotBeEmpty());
    }

    [Fact]
    public void ShouldThrow_WhenCorrectExceptionThrown_ReturnsException()
    {
        Action action = () => throw new InvalidOperationException("boom");
        var ex = action.ShouldThrow<InvalidOperationException>();
        Assert.Equal("boom", ex.Message);
    }

    [Fact]
    public void ShouldThrow_WhenNoExceptionThrown_Throws()
    {
        Action action = () => { };
        Assert.Throws<AssertionException>(() => action.ShouldThrow<InvalidOperationException>());
    }

    [Fact]
    public void ShouldThrow_WhenWrongExceptionThrown_Throws()
    {
        Action action = () => throw new ArgumentException("wrong");
        Assert.Throws<AssertionException>(() => action.ShouldThrow<InvalidOperationException>());
    }

    [Fact]
    public void ShouldBeGreaterThan_WhenGreater_DoesNotThrow()
    {
        10.ShouldBeGreaterThan(5);
    }

    [Fact]
    public void ShouldBeGreaterThan_WhenEqual_Throws()
    {
        Assert.Throws<AssertionException>(() => 5.ShouldBeGreaterThan(5));
    }

    [Fact]
    public void ShouldBeGreaterThan_WhenLess_Throws()
    {
        Assert.Throws<AssertionException>(() => 3.ShouldBeGreaterThan(5));
    }

    [Fact]
    public void ShouldBeLessThan_WhenLess_DoesNotThrow()
    {
        3.ShouldBeLessThan(5);
    }

    [Fact]
    public void ShouldBeLessThan_WhenEqual_Throws()
    {
        Assert.Throws<AssertionException>(() => 5.ShouldBeLessThan(5));
    }

    [Fact]
    public void ShouldBeLessThan_WhenGreater_Throws()
    {
        Assert.Throws<AssertionException>(() => 10.ShouldBeLessThan(5));
    }
}
