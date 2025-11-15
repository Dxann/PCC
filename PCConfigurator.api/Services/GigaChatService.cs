using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class GigaChatService
{
    private readonly HttpClient _http;
    private readonly GigaChatAuthService _auth;

    public GigaChatService(HttpClient http, GigaChatAuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<string> AskAsync(string question)
    {
        string token = await _auth.GetTokenAsync();

        string prompt = $@"
            Ты — опытный консультант по подбору и сборке компьютеров. Твоя задача — давать точные, полезные и структурированные ответы на вопросы, связанные с компьютерными компонентами, их совместимостью, производительностью и оптимизацией сборок.

            Ответь на следующий вопрос пользователя, максимально подробно, но четко. 

            Если вопрос не связан с ПК — вежливо скажи, что ты отвечаешь только на вопросы по сборке ПК. На вопросы не по теме, не отвечай.
            ";

        var body = new
        {
            model = "GigaChat",
            messages = new[]
            {
                new { role = "system", content = prompt },
                new { role = "user", content = question }
            },
            temperature = 0.5
        };

        var json = JsonSerializer.Serialize(body);
        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://gigachat.devices.sberbank.ru/api/v1/chat/completions");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(text);
        var answer = doc.RootElement
                       .GetProperty("choices")[0]
                       .GetProperty("message")
                       .GetProperty("content").GetString();

        return answer;
    }
}
