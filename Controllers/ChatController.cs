using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Chat.Request;

namespace PapisPowerPracticeMvc.Controllers
{
	[Route("[controller]")]
	public class ChatController : Controller
	{
		private readonly IChatService _chatService;
		private readonly ILogger<ChatController> _logger;

		public ChatController(IChatService chatService, ILogger<ChatController> logger)
		{
			_chatService = chatService;
			_logger = logger;
		}

		[HttpGet("Messages")]
		public async Task<IActionResult> Messages()
		{
			try
			{
				var messages = await _chatService.GetMessagesAsync();
				return Json(messages);
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Failed to load chat messages");
				return StatusCode(500, "Failed to load messages");
			}
		}

		[HttpPost("SendMessage")]
		public async Task<IActionResult> SendMessage([FromBody] ChatRequestDTO request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Message))
			{
				return BadRequest(new { success = false, error = "Message cannot be empty" });
			}

			try
			{
				var assistant = await _chatService.SendMessageAsync(request.Message);
				return Json(new { success = true, assistant = assistant.Message });
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "Failed to send chat message");
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}
	}
}
