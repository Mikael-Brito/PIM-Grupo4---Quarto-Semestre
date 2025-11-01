using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyProject.Data;
using MyProject.Models;
using System.Linq;

namespace MyProject.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // LISTAR USUÁRIOS
        // ============================
        [Authorize] // Mantido, pois é uma tela de gerenciamento que deve ser restrita
        public IActionResult Index()
        {
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
            // Mantendo a verificação de login, pois o [Authorize] garante que a sessão existe, mas a lógica de filtragem depende do ID.
            if (!usuarioIdLogado.HasValue)
                return RedirectToAction("Login", "Login");

            var usuarios = _context.Usuarios
                    .Where(u => u.CriadorId == usuarioIdLogado || u.Id == usuarioIdLogado)
                    .ToList();

            return View(usuarios);
        }

        // ============================
        // CRIAR ADMIN (GET)
        // ============================
        public IActionResult CriarAdmin()
        {
            //Permite a criação de Admin apenas se não houver Admin existente
            //if (_context.Usuarios.Any(u => u.IsAdmin))
            //{
            //    TempData["Erro"] = "Já existe um administrador cadastrado. Por favor, faça login para gerenciar usuários.";
            //    return RedirectToAction("Login", "Login");
            //}

            return View(); // Vai usar a View CriarAdmin.cshtml
        }

        // ============================
        // CRIAR ADMIN (POST)
        // ============================
        [HttpPost]
        public IActionResult CriarAdmin(UsuarioModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            // Garante que é o primeiro Admin
            //if (_context.Usuarios.Any(u => u.IsAdmin))
            //{
            //    TempData["Erro"] = "Já existe um administrador cadastrado.";
            //    return RedirectToAction("Login", "Login");
            //}

            usuario.IsAdmin = true;
            usuario.CriadorId = null; // nenhum criador

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Login automático do admin criado
            HttpContext.Session.SetInt32("UsuarioLogadoId", usuario.Id);
            HttpContext.Session.SetString("UsuarioLogadoNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioLogadoEmail", usuario.Email);
            HttpContext.Session.SetString("UsuarioLogadoIsAdmin", "True");

            // Redirecionamento para a tela de confirmação de compra (mantendo o fluxo original)
            // Simula que a criação do Admin está ligada à compra de plano
            // O plano será fixo para manter a compatibilidade com a ConfirmacaoCompra
            return RedirectToAction("ConfirmacaoCompra", new { usuarioId = usuario.Id, plano = "plano1" });
        }

        // ============================
        // CRIAR USUÁRIO NORMAL (GET)
        // ============================
        // Mantendo [Authorize] e a verificação de Admin para garantir que apenas Admins possam criar usuários
        [Authorize] 
        public IActionResult CriarUsuario()
        {
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
            var isAdmin = HttpContext.Session.GetString("UsuarioLogadoIsAdmin") == "True";

            // Exige que um Admin esteja logado
            if (!usuarioIdLogado.HasValue || !isAdmin)
            {
                TempData["Erro"] = "Você precisa estar logado como Admin para criar novos usuários.";
                return RedirectToAction("Login", "Login");
            }

            return View(); // Vai usar a View CriarUsuarioNormal.cshtml
        }

        // ============================
        // CRIAR USUÁRIO NORMAL (POST)
        // ============================
        [Authorize]
        [HttpPost]
        public IActionResult CriarUsuario(UsuarioModel usuario)
        {
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
            var isAdmin = HttpContext.Session.GetString("UsuarioLogadoIsAdmin") == "True";

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            // Exige que um Admin esteja logado
            if (!usuarioIdLogado.HasValue || !isAdmin)
            {
                TempData["Erro"] = "Você precisa estar logado como Admin para criar novos usuários.";
                return RedirectToAction("Login", "Login");
            }

            // Criação de Usuário Normal
            usuario.IsAdmin = false;
            // Vincula o novo usuário ao Admin logado
            usuario.CriadorId = usuarioIdLogado;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ============================
        // CONFIRMAÇÃO DE COMPRA
        // ============================
        public IActionResult ConfirmacaoCompra(int usuarioId, string plano)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null);

            ViewBag.Plano = plano;
            ViewBag.Usuario = usuario;
            return View();
        }

        // ============================
        // EDITAR (GET)
        // ============================
        [Authorize] // Mantido, pois é uma tela de gerenciamento que deve ser restrita
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // ============================
        // EDITAR (POST)
        // ============================
        [Authorize] // Mantido
        [HttpPost]
        public IActionResult Editar(UsuarioModel usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            var usuarioExistente = _context.Usuarios.Find(usuario.Id);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Contato = usuario.Contato;

            if (!string.IsNullOrEmpty(usuario.Senha))
                usuarioExistente.Senha = usuario.Senha;

            _context.Usuarios.Update(usuarioExistente);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ============================
        // APAGAR (GET)
        // ============================
        [Authorize] // Mantido, pois é uma tela de gerenciamento que deve ser restrita
        public IActionResult Apagar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // ============================
        // APAGAR (POST)
        // ============================
        [Authorize] // Mantido
        [HttpPost]
        public IActionResult ConfirmarApagar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
