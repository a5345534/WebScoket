namespace WebSocketService.Enums
{
    public enum DtoType
    {
        ProgramConnect = 0,
        CreateChatRoom = 1,
        JoinChatRoom = 2,
        RecivedMessage = 3,
        LeaveChatRoom = 4,
        BroadcastMessage = 5,
        PingDto = 51,
        PongDto = 52,
        ProgramClose = 90,
        Error = 99,

    }
}
