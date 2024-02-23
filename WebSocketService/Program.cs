
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
            builder.WebHost.UseKestrel(options =>
            {
                options.ListenAnyIP(5000); // 监听所有网络接口上的端口 5000
            }); ;

            var app = builder.Build();
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>(); // 使用中间件
            app.UseHttpsRedirection();       

            app.Run();
        }
    }
}