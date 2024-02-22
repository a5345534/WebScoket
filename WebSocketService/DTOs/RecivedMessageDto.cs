using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class RecivedMessageDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.RecivedMessage;
        public string chatRoomId { get; }
        public string message { get; }
        public DateTime messageTimestamp { get; } = DateTime.Now;
        public RecivedMessageDto(string ChatRoomId, string Message)
        {
            //this.userId = UserId;
            //this.userName = UserName;
            this.chatRoomId = ChatRoomId;
            this.message = Message;
            //this.messageTimestamp = DateTime.Now;
        }
    }
}
