using Microsoft.AspNetCore.Mvc;
using System;

namespace MyProject.Controllers
{
    public class ChatbotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
