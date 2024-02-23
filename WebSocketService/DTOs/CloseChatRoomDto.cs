using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class CloseChatRoomDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.CloseChatRoom;
        public string chatRoomId { get; }
        public CloseChatRoomDto(string ChatRoomId)
        {
            chatRoomId = ChatRoomId;
        }
    }
}
