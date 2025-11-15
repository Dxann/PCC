using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class GigaChatAuthService
{
    private readonly HttpClient _http;
    private string _cachedToken;
    private DateTime _tokenExpiration;

    private readonly string _authKey = "MDE5YTg3NzctMmRiMi03NzkxLWJkYWUtZjY0NTI5OTExMGI5OmE2NGJiMDkxLWRjZWEtNDEwMi1hYzI4LTU1NzQ0MDM4N2VmOQ==";

    public GigaChatAuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiration)
            return _cachedToken;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://ngw.devices.sberbank.ru:9443/api/v2/oauth");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("RqUID", Guid.NewGuid().ToString());
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _authKey);

        request.Content = new StringContent("scope=GIGACHAT_API_PERS", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unexpected status code {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("access_token", out var accessTokenProp))
            throw new InvalidOperationException("Access token is missing from the server's response.");

        _cachedToken = accessTokenProp.GetString();

        if (root.TryGetProperty("expires_at", out var expiresAtProp))
        {
            var expiresAtUnixMs = expiresAtProp.GetInt64();
            _tokenExpiration = DateTimeOffset.FromUnixTimeMilliseconds(expiresAtUnixMs).UtcDateTime.AddSeconds(-60);
        }
        else if (root.TryGetProperty("expires_in", out var expiresInProp))
        {
            int expires = expiresInProp.GetInt32();
            _tokenExpiration = DateTime.UtcNow.AddSeconds(expires - 60);
        }
        else
        {
            throw new InvalidOperationException("Expiration info is missing from the server's response.");
        }

        return _cachedToken;
    }
}
