using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class PingDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.PingDto;
    }
}
