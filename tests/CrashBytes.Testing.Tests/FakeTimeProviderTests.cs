using Xunit;

namespace CrashBytes.Testing.Tests;

public class FakeTimeProviderTests
{
    [Fact]
    public void DefaultConstructor_InitializesToCurrentTime()
    {
        var before = DateTimeOffset.UtcNow;
        var provider = new FakeTimeProvider();
        var after = DateTimeOffset.UtcNow;

        Assert.InRange(provider.Now, before, after);
    }

    [Fact]
    public void Constructor_WithSpecificTime_SetsCorrectly()
    {
        var time = new DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(time);

        Assert.Equal(time, provider.Now);
    }

    [Fact]
    public void Advance_MovesTimeForward()
    {
        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(start);

        provider.Advance(TimeSpan.FromHours(2));

        Assert.Equal(start.AddHours(2), provider.Now);
    }

    [Fact]
    public void Advance_MultipleTimes_Accumulates()
    {
        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var provider = new FakeTimeProvider(start);

        provider.Advance(TimeSpan.FromMinutes(30));
        provider.Advance(TimeSpan.FromMinutes(45));

        Assert.Equal(start.AddMinutes(75), provider.Now);
    }

    [Fact]
    public void Advance_WithNegativeDuration_Throws()
    {
        var provider = new FakeTimeProvider();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            provider.Advance(TimeSpan.FromSeconds(-1)));
    }

    [Fact]
    public void Set_ChangesTimeToSpecificValue()
    {
        var provider = new FakeTimeProvider();
        var target = new DateTimeOffset(2030, 6, 15, 12, 0, 0, TimeSpan.Zero);

        provider.Set(target);

        Assert.Equal(target, provider.Now);
    }

    [Fact]
    public void Set_CanMoveBackward()
    {
        var provider = new FakeTimeProvider(new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero));
        var earlier = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);

        provider.Set(earlier);

        Assert.Equal(earlier, provider.Now);
    }

    [Fact]
    public void ImplementsITimeProvider()
    {
        ITimeProvider provider = new FakeTimeProvider(new DateTimeOffset(2026, 3, 5, 0, 0, 0, TimeSpan.Zero));
        Assert.Equal(2026, provider.Now.Year);
    }

    [Fact]
    public void Advance_ReturnsSelf_ForChaining()
    {
        var provider = new FakeTimeProvider();
        var result = provider.Advance(TimeSpan.FromSeconds(1));
        Assert.Same(provider, result);
    }

    [Fact]
    public void Set_ReturnsSelf_ForChaining()
    {
        var provider = new FakeTimeProvider();
        var result = provider.Set(DateTimeOffset.UtcNow);
        Assert.Same(provider, result);
    }
}
