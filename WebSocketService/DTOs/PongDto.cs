using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class PongDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.PongDto;
    }
}
