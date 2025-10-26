using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyProject.Controllers
{
    [Authorize]
    public class ChatbotController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Chatbot de Suporte";
            return View();
        }
    }
}
