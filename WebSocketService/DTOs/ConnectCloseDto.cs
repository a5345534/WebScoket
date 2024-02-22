using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class ConnectCloseDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.ProgramClose;
    }
}
