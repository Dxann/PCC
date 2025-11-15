using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatBotController : ControllerBase
{
    private readonly GigaChatService _service;

    public ChatBotController(GigaChatService service)
    {
        _service = service;
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] ChatRequest req)
    {

        if (string.IsNullOrWhiteSpace(req.Question))
            return BadRequest(new { error = "Empty question" });

        var answer = await _service.AskAsync(req.Question);
        return Ok(new { answer });
    }
}
