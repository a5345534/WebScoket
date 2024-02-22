using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class LeaveChatRoomDto : MasterDto
    {
        public DtoType dtoType { get;  } = DtoType.LeaveChatRoom;
        public string chatRoomId { get; set; }
    }
}
