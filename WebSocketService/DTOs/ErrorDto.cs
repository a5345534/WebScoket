using WebSocketService.Enums;

namespace WebSocketService.DTOs
{
    public class ErrorDto : MasterDto
    {
        public DtoType dtoType { get; } = DtoType.Error;
    }
}
