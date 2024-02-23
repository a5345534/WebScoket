using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class CreateChatRoomDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.CreateChatRoom;
        public string chatRoomId { get; }
        public List<UserDto> users { get; }
        public CreateChatRoomDto(string chatRoomId,List<UserDto> Users)
        {
            this.chatRoomId = chatRoomId;
            this.users = Users;
        }
    }
}
