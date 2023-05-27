using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using BNetInstaller.Constants;
using System.Text.Json;

namespace BNetInstaller;

internal sealed class AgentClient : IDisposable
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public AgentClient(int port)
    {
        _client = new();
        _client.DefaultRequestHeaders.Add("User-Agent", "phoenix-agent/1.0");
        _client.BaseAddress = new Uri($"http://127.0.0.1:{port}");

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

    public async Task<HttpResponseMessage> SendAsync(string endpoint, HttpVerb verb, string content = null)
    {
        var request = new HttpRequestMessage(new(verb.ToString()), endpoint);

        if (verb != HttpVerb.GET && !string.IsNullOrEmpty(content))
            request.Content = new StringContent(content);

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            await HandleRequestFailure(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendAsync<T>(string endpoint, HttpVerb verb, T payload = null) where T : class
    {
        if (payload == null)
            return await SendAsync(endpoint, verb);
        else
            return await SendAsync(endpoint, verb, JsonSerializer.Serialize(payload, _serializerOptions));
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
        
        Span<char> lower = stackalloc char[0x100];
        Span<char> output = stackalloc char[0x100];

        name.AsSpan().ToLowerInvariant(lower);

        var length = 0;

        for (var i = 0; i < name.Length; i++)
        {
            if (i != 0 && name[i] is >= 'A' and <= 'Z')
                output[length++] = '_';

            output[length++] = lower[i];
        }

        return new string(output[..length]);
    }
}
