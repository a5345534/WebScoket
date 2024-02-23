using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class UpdateChatRoomDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.UpdateChatRoom;
        public string chatRoomId { get; }
        public List<UserDto> users { get; }

        public UpdateChatRoomDto(string ChatRoomId, List<UserDto> Users)
        {
            chatRoomId = ChatRoomId;
            users = Users;
        }
    }
}
