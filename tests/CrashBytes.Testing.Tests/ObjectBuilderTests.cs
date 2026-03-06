using Xunit;

namespace CrashBytes.Testing.Tests;

public class ObjectBuilderTests
{
    public class TestPerson
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    [Fact]
    public void Create_ReturnsBuilderInstance()
    {
        var builder = ObjectBuilder<TestPerson>.Create();
        Assert.NotNull(builder);
    }

    [Fact]
    public void Build_WithNoConfiguration_ReturnsDefaultObject()
    {
        var person = ObjectBuilder<TestPerson>.Create().Build();

        Assert.Equal(string.Empty, person.Name);
        Assert.Equal(0, person.Age);
    }

    [Fact]
    public void With_Expression_SetsProperty()
    {
        var person = ObjectBuilder<TestPerson>.Create()
            .With(p => p.Name, "Alice")
            .With(p => p.Age, 30)
            .Build();

        Assert.Equal("Alice", person.Name);
        Assert.Equal(30, person.Age);
    }

    [Fact]
    public void With_Action_ConfiguresObject()
    {
        var person = ObjectBuilder<TestPerson>.Create()
            .With(p =>
            {
                p.Name = "Bob";
                p.Age = 25;
                p.Email = "bob@test.com";
            })
            .Build();

        Assert.Equal("Bob", person.Name);
        Assert.Equal(25, person.Age);
        Assert.Equal("bob@test.com", person.Email);
    }

    [Fact]
    public void With_OverridesEarlierConfiguration()
    {
        var person = ObjectBuilder<TestPerson>.Create()
            .With(p => p.Name, "First")
            .With(p => p.Name, "Second")
            .Build();

        Assert.Equal("Second", person.Name);
    }

    [Fact]
    public void BuildMany_ReturnsCorrectCount()
    {
        var people = ObjectBuilder<TestPerson>.Create()
            .With(p => p.Name, "Template")
            .BuildMany(5);

        Assert.Equal(5, people.Count);
        Assert.All(people, p => Assert.Equal("Template", p.Name));
    }

    [Fact]
    public void BuildMany_WithCustomizer_AppliesIndex()
    {
        var people = ObjectBuilder<TestPerson>.Create()
            .BuildMany(3, (p, i) =>
            {
                p.Name = $"Person-{i}";
                p.Age = 20 + i;
            });

        Assert.Equal("Person-0", people[0].Name);
        Assert.Equal("Person-1", people[1].Name);
        Assert.Equal("Person-2", people[2].Name);
        Assert.Equal(20, people[0].Age);
        Assert.Equal(22, people[2].Age);
    }

    [Fact]
    public void BuildMany_WithZeroCount_ReturnsEmptyList()
    {
        var people = ObjectBuilder<TestPerson>.Create().BuildMany(0);
        Assert.Empty(people);
    }

    [Fact]
    public void BuildMany_WithNegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ObjectBuilder<TestPerson>.Create().BuildMany(-1));
    }

    [Fact]
    public void Build_CreatesIndependentInstances()
    {
        var builder = ObjectBuilder<TestPerson>.Create()
            .With(p => p.Name, "Shared");

        var a = builder.Build();
        var b = builder.Build();

        a.Name = "Modified";
        Assert.Equal("Shared", b.Name);
    }
}
