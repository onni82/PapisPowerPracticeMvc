using System.Net.Http.Json;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Chat.Request;
using PapisPowerPracticeMvc.Models.Chat.Response;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChatMsgDTO> SendMessageAsync(ChatRequestDTO request)
        {
            var resp = await _httpClient.PostAsJsonAsync("ChatBot/chat", request);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Chat API error: {resp.StatusCode} - {body}");
            }

            var result = await resp.Content.ReadFromJsonAsync<ChatMsgDTO>();
            return result ?? throw new InvalidOperationException("Chat API returned empty response");
        }

        public async Task<IEnumerable<ChatMsgDTO>> GetSessionMessagesAsync(Guid sessionId)
        {
            var resp = await _httpClient.GetAsync($"ChatBot/{sessionId}");
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Chat API error: {resp.StatusCode} - {body}");
            }

            var result = await resp.Content.ReadFromJsonAsync<IEnumerable<ChatMsgDTO>>();
            return result ?? Enumerable.Empty<ChatMsgDTO>();
        }

        public async Task<IEnumerable<ChatSessionDTO>> GetUserSessionsAsync(string userId)
        {
            var resp = await _httpClient.GetAsync($"ChatBot/sessions/{Uri.EscapeDataString(userId)}");
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Chat API error: {resp.StatusCode} - {body}");
            }

            var result = await resp.Content.ReadFromJsonAsync<IEnumerable<ChatSessionDTO>>();
            return result ?? Enumerable.Empty<ChatSessionDTO>();
        }
    }
}
