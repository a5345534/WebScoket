using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using WebSocketService.DTOs;
using System.Threading;

namespace WebSocketService.Classes
{
    public class ConnectedUser
    {
        public static readonly TimeSpan heartLifeTime = new TimeSpan(0, 1, 0);
        public string id { get; }
        public string name { get; }
        public WebSocket webSocket { get; set; }
        public DateTime lastPong { get; set; }
        public System.Timers.Timer heartbeatTimer { get; set; }

        public ConnectedUser(string Name, WebSocket webSocket)
        {
            id = Guid.NewGuid().ToString();
            name = Name;
            this.webSocket = webSocket;
            lastPong = DateTime.Now;
            heartbeatTimer = new System.Timers.Timer(heartLifeTime * 0.5);
        }

        public async Task ListenMessage<T>(T recivedDto) where T : MasterDto
        {
            var jsonString = JsonSerializer.Serialize(recivedDto);
            var buffer = Encoding.UTF8.GetBytes(jsonString);
            var segment = new ArraySegment<byte>(buffer);
            webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async Task<string> SendJsonMessage(CancellationTokenSource websocketCts)
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            using (var ms = new MemoryStream())
            {

                WebSocketReceiveResult result;

                result = await webSocket.ReceiveAsync(buffer, websocketCts.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("收到Close訊號");
                    return JsonSerializer.Serialize(new ConnectCloseDto());
                }

                do
                {
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);


                ms.Seek(0, System.IO.SeekOrigin.Begin);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    using (var reader = new System.IO.StreamReader(ms, Encoding.UTF8))
                    {
                        var dtoJson = await reader.ReadToEndAsync();
                        return dtoJson;
                    }
                }
                else
                {
                    return ""; // 非文本消息，这里可能需要其他处理
                }
            }
        }
        public async Task IsHeartStop(CancellationTokenSource websocketCts)
        {
            if (lastPong + ConnectedUser.heartLifeTime < DateTime.Now)
                websocketCts.Cancel();
        }
    }
}
