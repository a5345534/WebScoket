using Microsoft.AspNetCore.Mvc;

namespace WebSocketService.Controllers
{
    public class WebSocketService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
