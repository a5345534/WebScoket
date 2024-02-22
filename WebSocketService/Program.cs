
using System.Net.WebSockets;
using System.Text;

namespace WebSocketService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.WebHost.UseUrls("http://*:5000");
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>(); // 使用中间件
            //app.Use(async (context, next) =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {
            //        MyWebSocketManager webSocketManager = new MyWebSocketManager();
            //        await webSocketManager.HandleWebSocketCommunication(context);
            //    }
            //    else
            //    {
            //        await next(); // 如果不是 WebSocket 請求，則繼續處理管道中的下一個中間件
            //    }
            //});
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var serverMsg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Message from Client: {serverMsg}"); // 在控制台上打印消息

                // 假設你只想打印消息，不回發給客戶端，這裡可以不做回發
                // 如果需要回發消息到客戶端，可以使用 webSocket.SendAsync 方法

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}