using System.Diagnostics;

namespace BNetInstaller;

internal sealed class AgentClient : IDisposable
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public AgentClient(int port)
    {
        _client = new();
        _client.DefaultRequestHeaders.Add("User-Agent", "phoenix-agent/1.0");
        _client.BaseAddress = new($"http://127.0.0.1:{port}");

        _serializerOptions = new()
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
            DictionaryKeyPolicy = SnakeCaseNamingPolicy.Instance,
        };
    }

    public void SetAuthorization(string authorization)
    {
        _client.DefaultRequestHeaders.Add("Authorization", authorization);
    }

    public async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, string content = null)
    {
        var request = new HttpRequestMessage(method, endpoint);

        if (method != HttpMethod.Get && !string.IsNullOrEmpty(content))
            request.Content = new StringContent(content);

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            await HandleRequestFailure(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendAsync<T>(string endpoint, HttpMethod method, T payload = null) where T : class
    {
        if (payload == null)
            return await SendAsync(endpoint, method);
        else
            return await SendAsync(endpoint, method, JsonSerializer.Serialize(payload, _serializerOptions));
    }

    private static async Task HandleRequestFailure(HttpResponseMessage response)
    {
        var uri = response.RequestMessage.RequestUri.AbsolutePath;
        var statusCode = response.StatusCode;
        var content = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"{(int)statusCode} {statusCode}: {uri} {content}");
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}

file class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static readonly SnakeCaseNamingPolicy Instance = new();

    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;
        
        Span<char> input = stackalloc char[0x100];
        Span<char> output = stackalloc char[0x100];

        var inputLen = name.AsSpan().ToLowerInvariant(input);
        var outputLen = 0;

        for (var i = 0; i < inputLen; i++)
        {
            if (i != 0 && name[i] is >= 'A' and <= 'Z')
                output[outputLen++] = '_';

            output[outputLen++] = input[i];
        }

        return new string(output[..outputLen]);
    }
}
