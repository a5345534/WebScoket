using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class JoinChatRoomDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.JoinChatRoom;
        public string chatRoomId { get; set; }
    }
}
