using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class BroadcastMessageDto : RecivedMessageDto
    {
        public DtoType dtoType { get; } = DtoType.BroadcastMessage; 
        public string massengerSenderName { get; }
        public BroadcastMessageDto(string massengerSenderName, RecivedMessageDto message) : base(message.chatRoomId, message.message)
        {
            this.massengerSenderName = massengerSenderName;
        }
    }
}
