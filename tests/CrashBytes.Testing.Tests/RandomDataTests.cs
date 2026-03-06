using Xunit;

namespace CrashBytes.Testing.Tests;

public class RandomDataTests
{
    public RandomDataTests()
    {
        // Reset seed before each test class for determinism
        RandomData.WithSeed(12345);
    }

    [Fact]
    public void String_ReturnsCorrectLength()
    {
        var result = RandomData.String(20);
        Assert.Equal(20, result.Length);
    }

    [Fact]
    public void String_DefaultLength_ReturnsTenChars()
    {
        var result = RandomData.String();
        Assert.Equal(10, result.Length);
    }

    [Fact]
    public void String_ZeroLength_ReturnsEmpty()
    {
        var result = RandomData.String(0);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void String_NegativeLength_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomData.String(-1));
    }

    [Fact]
    public void Email_ReturnsValidFormat()
    {
        var email = RandomData.Email();
        Assert.Contains("@", email);
        Assert.Contains(".", email);
    }

    [Fact]
    public void Url_ReturnsValidFormat()
    {
        var url = RandomData.Url();
        Assert.StartsWith("https://", url);
    }

    [Fact]
    public void Int_ReturnsWithinRange()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = RandomData.Int(10, 20);
            Assert.InRange(value, 10, 20);
        }
    }

    [Fact]
    public void Guid_ReturnsNonEmptyGuid()
    {
        var guid = RandomData.Guid();
        Assert.NotEqual(System.Guid.Empty, guid);
    }

    [Fact]
    public void Bool_ReturnsBooleanValue()
    {
        // Run multiple times to ensure both values are possible
        var results = Enumerable.Range(0, 50).Select(_ => RandomData.Bool()).ToList();
        Assert.Contains(true, results);
        Assert.Contains(false, results);
    }

    [Fact]
    public void Pick_ReturnsItemFromList()
    {
        var items = new List<string> { "a", "b", "c" };
        var picked = RandomData.Pick(items);
        Assert.Contains(picked, items);
    }

    [Fact]
    public void Pick_EmptyList_Throws()
    {
        Assert.Throws<ArgumentException>(() => RandomData.Pick(new List<string>()));
    }

    [Fact]
    public void Pick_NullList_Throws()
    {
        Assert.Throws<ArgumentException>(() => RandomData.Pick<string>(null!));
    }

    [Fact]
    public void WithSeed_ProducesReproducibleResults()
    {
        RandomData.WithSeed(999);
        var first = RandomData.String(10);
        var firstInt = RandomData.Int(0, 100);

        RandomData.WithSeed(999);
        var second = RandomData.String(10);
        var secondInt = RandomData.Int(0, 100);

        Assert.Equal(first, second);
        Assert.Equal(firstInt, secondInt);
    }

    [Fact]
    public void DifferentSeeds_ProduceDifferentResults()
    {
        RandomData.WithSeed(1);
        var first = RandomData.String(20);

        RandomData.WithSeed(2);
        var second = RandomData.String(20);

        Assert.NotEqual(first, second);
    }
}
