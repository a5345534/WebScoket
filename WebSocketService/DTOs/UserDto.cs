using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class UserDto
    {
        public string userId { get; }
        public string userName { get; }
        public UserDto(string UserId, string UserName)
        {
            userId = UserId;
            userName = UserName;
        }
    }
}
