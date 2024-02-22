using Microsoft.AspNetCore.Http;

namespace WebSocketService
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private MyWebSocketManager _webSocketManager;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
            _webSocketManager = new MyWebSocketManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                switch (context.Request.Path)
                {
                    case "/ws/newConnecting":
                        await _webSocketManager.HandelNewConnect(context);
                        break;
                    case "/ws/createChatRoom":
                        //await _webSocketManager.HandelCreateChatRoom(context);
                        break;
                    case "/ws/newMessage":
                        //await _webSocketManager.HandleNewMessage(context);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // 如果不是 WebSocket 请求，则继续处理管道中的下一个中间件
                await _next(context);
            }
        }
    }
}
