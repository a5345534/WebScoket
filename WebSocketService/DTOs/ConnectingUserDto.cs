using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class ConnectingUserDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.ProgramConnect;
    }
}
