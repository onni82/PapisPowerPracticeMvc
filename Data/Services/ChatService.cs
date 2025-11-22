using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Chat.Response;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChatService> _logger;

        public ChatService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<ChatService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ChatMsgDTO> SendMessageAsync(string message)
        {
            var payload = new { Message = message ?? string.Empty };

            var response = await _httpClient.PostAsJsonAsync("ChatBot/chat", payload);
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Chat API returned error: {response.StatusCode} - {text}");
            }

            var result = await response.Content.ReadFromJsonAsync<ChatMsgDTO>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null) throw new InvalidOperationException("Chat API returned empty response");
            return result;
        }

        public async Task<IEnumerable<ChatMsgDTO>> GetMessagesAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null)
            {
                throw new InvalidOperationException("User not authenticated");
            }

            var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                         ?? httpContext.User.FindFirst("sub")?.Value
                         ?? httpContext.User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("Unable to determine current user id");
            }

            var url = $"ChatBot/messages/{Uri.EscapeDataString(userId)}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Chat API returned error: {response.StatusCode} - {text}");
            }

            var messages = await response.Content.ReadFromJsonAsync<IEnumerable<ChatMsgDTO>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return messages ?? Array.Empty<ChatMsgDTO>();
        }
    }
}
