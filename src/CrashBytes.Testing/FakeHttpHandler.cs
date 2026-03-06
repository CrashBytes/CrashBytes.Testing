using System.Net;
using System.Text;
using System.Text.Json;

namespace CrashBytes.Testing;

/// <summary>
/// A fake <see cref="HttpMessageHandler"/> that returns preconfigured responses
/// and captures outgoing requests for assertion.
/// </summary>
public sealed class FakeHttpHandler : HttpMessageHandler
{
    private readonly Dictionary<string, (HttpStatusCode StatusCode, string Content, string ContentType)> _urlResponses = new(StringComparer.OrdinalIgnoreCase);
    private (HttpStatusCode StatusCode, string Content, string ContentType) _defaultResponse = (HttpStatusCode.OK, string.Empty, "text/plain");
    private readonly List<HttpRequestMessage> _requests = new();

    /// <summary>
    /// Gets the list of all captured requests.
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> Requests => _requests.AsReadOnly();

    /// <summary>
    /// Gets the most recent request, or null if no requests have been made.
    /// </summary>
    public HttpRequestMessage? LastRequest => _requests.Count > 0 ? _requests[^1] : null;

    /// <summary>
    /// Sets the default response returned for any URL without a specific mapping.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="content">The response body content.</param>
    public FakeHttpHandler RespondWith(HttpStatusCode statusCode, string content = "")
    {
        _defaultResponse = (statusCode, content, "text/plain");
        return this;
    }

    /// <summary>
    /// Sets a response for a specific URL.
    /// </summary>
    /// <param name="url">The URL to match (case-insensitive).</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="content">The response body content.</param>
    public FakeHttpHandler RespondWith(string url, HttpStatusCode statusCode, string content = "")
    {
        _urlResponses[url] = (statusCode, content, "text/plain");
        return this;
    }

    /// <summary>
    /// Sets the default response with a JSON-serialized body.
    /// </summary>
    /// <typeparam name="T">The type to serialize.</typeparam>
    /// <param name="data">The data to serialize as the response body.</param>
    /// <param name="statusCode">The HTTP status code (defaults to 200 OK).</param>
    public FakeHttpHandler RespondWithJson<T>(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        _defaultResponse = (statusCode, json, "application/json");
        return this;
    }

    /// <summary>
    /// Creates an <see cref="HttpClient"/> backed by this fake handler.
    /// </summary>
    public HttpClient CreateClient() => new(this)
    {
        BaseAddress = new Uri("https://fake.example.com")
    };

    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);

        var url = request.RequestUri?.ToString() ?? string.Empty;
        var (statusCode, content, contentType) = _urlResponses.TryGetValue(url, out var specific)
            ? specific
            : _defaultResponse;

        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, contentType),
            RequestMessage = request
        };

        return Task.FromResult(response);
    }
}
