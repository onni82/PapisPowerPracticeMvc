using System.Collections.Generic;
using System.Threading.Tasks;
using PapisPowerPracticeMvc.Models.Chat.Request;
using PapisPowerPracticeMvc.Models.Chat.Response;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMsgDTO>> GetMessagesAsync();
        Task<ChatMsgDTO> SendMessageAsync(string message);
    }
}