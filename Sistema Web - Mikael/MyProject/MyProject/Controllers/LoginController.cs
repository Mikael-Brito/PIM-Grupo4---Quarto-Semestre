using Microsoft.AspNetCore.Mvc;

namespace MyProject.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
