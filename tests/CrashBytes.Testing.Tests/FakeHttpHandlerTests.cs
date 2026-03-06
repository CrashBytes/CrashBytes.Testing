using System.Net;
using Xunit;

namespace CrashBytes.Testing.Tests;

public class FakeHttpHandlerTests
{
    [Fact]
    public async Task CreateClient_ReturnsWorkingClient()
    {
        var handler = new FakeHttpHandler();
        using var client = handler.CreateClient();

        var response = await client.GetAsync("/test");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RespondWith_SetsDefaultResponse()
    {
        var handler = new FakeHttpHandler()
            .RespondWith(HttpStatusCode.NotFound, "not found");

        using var client = handler.CreateClient();
        var response = await client.GetAsync("/anything");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("not found", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RespondWith_Url_ReturnsUrlSpecificResponse()
    {
        var handler = new FakeHttpHandler()
            .RespondWith(HttpStatusCode.OK, "default")
            .RespondWith("https://fake.example.com/api/users", HttpStatusCode.Created, "user created");

        using var client = handler.CreateClient();

        var defaultResponse = await client.GetAsync("/other");
        Assert.Equal(HttpStatusCode.OK, defaultResponse.StatusCode);

        var specificResponse = await client.GetAsync("https://fake.example.com/api/users");
        Assert.Equal(HttpStatusCode.Created, specificResponse.StatusCode);
        Assert.Equal("user created", await specificResponse.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RespondWithJson_SerializesObject()
    {
        var data = new { Name = "Test", Value = 42 };
        var handler = new FakeHttpHandler()
            .RespondWithJson(data);

        using var client = handler.CreateClient();
        var response = await client.GetAsync("/api/data");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("\"name\"", content);
        Assert.Contains("\"value\"", content);
        Assert.Contains("42", content);
    }

    [Fact]
    public async Task Requests_CapturesAllRequests()
    {
        var handler = new FakeHttpHandler();
        using var client = handler.CreateClient();

        await client.GetAsync("/first");
        await client.GetAsync("/second");
        await client.PostAsync("/third", new StringContent("body"));

        Assert.Equal(3, handler.Requests.Count);
    }

    [Fact]
    public async Task LastRequest_ReturnsLastRequest()
    {
        var handler = new FakeHttpHandler();
        using var client = handler.CreateClient();

        await client.GetAsync("/first");
        await client.GetAsync("/second");

        Assert.NotNull(handler.LastRequest);
        Assert.Contains("/second", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public void LastRequest_WhenNoRequests_ReturnsNull()
    {
        var handler = new FakeHttpHandler();
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public void Requests_WhenNoRequests_IsEmpty()
    {
        var handler = new FakeHttpHandler();
        Assert.Empty(handler.Requests);
    }
}
