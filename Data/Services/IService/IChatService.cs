using PapisPowerPracticeMvc.Models.Chat.Request;
using PapisPowerPracticeMvc.Models.Chat.Response;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IChatService
    {
        Task<ChatMsgDTO> SendMessageAsync(ChatRequestDTO request);
        Task<IEnumerable<ChatMsgDTO>> GetSessionMessagesAsync(Guid sessionId);
        Task<IEnumerable<ChatSessionDTO>> GetUserSessionsAsync(string userId);
    }
}