using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Email e senha são obrigatórios.";
                return View();
            }

            // Buscar o usuário no banco de dados
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Email ou senha incorretos.";
                return View();
            }

            // Criar claims para o usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("IsAdmin", usuario.IsAdmin.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // Fazer o login
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties).Wait();

            // Armazenar na sessão para uso fácil nas views (mantendo a compatibilidade com o código existente)
            HttpContext.Session.SetInt32("UsuarioLogadoId", usuario.Id);
            HttpContext.Session.SetString("UsuarioLogadoNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioLogadoEmail", usuario.Email);
            HttpContext.Session.SetString("UsuarioLogadoIsAdmin", usuario.IsAdmin.ToString());

            // Redirecionar para a página de usuários
            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult Logout()
        {
            // Fazer o logout
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            // Limpar a sessão
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

