using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketService.Classes
{
    public class ChatRoom
    {
        public string chatRoomId { get; set; }
        public ConcurrentDictionary<string, ConnectedUser> roomUsers { get; set; }
        public ChatRoom()
        {
            chatRoomId = Guid.NewGuid().ToString();
            roomUsers = new ConcurrentDictionary<string, ConnectedUser>();
        }
    }
}
