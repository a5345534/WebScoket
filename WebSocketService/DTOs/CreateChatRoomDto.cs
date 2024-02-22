using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class CreateChatRoomDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.CreateChatRoom;
        public string chatRoomId { get; }
        public CreateChatRoomDto(string chatRoomId)
        {
            this.chatRoomId = chatRoomId;
        }
    }
}
