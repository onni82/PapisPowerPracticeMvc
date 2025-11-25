namespace PapisPowerPracticeMvc.Models.Chat.Response
{
    public class ChatMsgDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public bool IsUserMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
