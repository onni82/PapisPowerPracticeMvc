using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Chat.Request;
using PapisPowerPracticeMvc.Models.Chat.Response;

namespace PapisPowerPracticeMvc.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // POST /Chat/SendMessage
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { success = false, error = "Message cannot be empty" });
            }

            try
            {
                var assistantMsg = await _chatService.SendMessageAsync(request);

                return Json(new
                {
                    success = true,
                    assistant = assistantMsg.Message,
                    sessionId = assistantMsg.ChatSessionId,
                    messageId = assistantMsg.Id,
                    timestamp = assistantMsg.Timestamp
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET /Chat/Session/{sessionId}
        [HttpGet("Session/{sessionId:guid}")]
        public async Task<IActionResult> GetSessionMessages(Guid sessionId)
        {
            try
            {
                var messages = await _chatService.GetSessionMessagesAsync(sessionId);
                return Ok(messages);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}
