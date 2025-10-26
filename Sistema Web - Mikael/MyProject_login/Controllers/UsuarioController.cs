using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize] // Apenas usuários logados podem acessar
        public IActionResult Index()
        {
            // O usuário logado só pode ver os usuários que ele criou ou a si mesmo
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
            
            if (!usuarioIdLogado.HasValue)
            {
                // Isso não deve acontecer devido ao [Authorize], mas é uma segurança
                return RedirectToAction("Login", "Login");
            }

            var usuarios = _context.Usuarios
                .Where(u => u.CriadorId == usuarioIdLogado || u.Id == usuarioIdLogado)
                .ToList();
            
            return View(usuarios);
        }

        public IActionResult Criar(string plano = "")
        {
            ViewBag.Plano = plano;
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario, string plano = "")
        {
            if (ModelState.IsValid)
            {
                // 1. Lógica para definir o primeiro usuário como Admin
                if (!_context.Usuarios.Any())
                {
                    usuario.IsAdmin = true;
                }

                // 2. Lógica para definir o CriadorId (se não for o primeiro admin)
                var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
                if (usuarioIdLogado.HasValue)
                {
                    usuario.CriadorId = usuarioIdLogado.Value;
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Armazenar o ID do usuário criado na sessão para referência
                // Nota: A sessão será limpa no login, mas o ID é necessário para a Confirmação
                HttpContext.Session.SetInt32("UsuarioCriadoId", usuario.Id);
                HttpContext.Session.SetString("PlanoSelecionado", plano);

                // Redirecionar para a página de confirmação de compra ou para o sistema
                if (!string.IsNullOrEmpty(plano))
                {
                    // Se for o primeiro usuário (Admin), já o logamos para ele poder adicionar outros
                    if (usuario.IsAdmin)
                    {
                        // Simular login para o primeiro admin
                        HttpContext.Session.SetInt32("UsuarioLogadoId", usuario.Id);
                        HttpContext.Session.SetString("UsuarioLogadoNome", usuario.Nome);
                        HttpContext.Session.SetString("UsuarioLogadoEmail", usuario.Email);
                        HttpContext.Session.SetString("UsuarioLogadoIsAdmin", usuario.IsAdmin.ToString());
                    }

                    return RedirectToAction("ConfirmacaoCompra", new { usuarioId = usuario.Id, plano = plano });
                }

                return RedirectToAction("Index");
            }
            ViewBag.Plano = plano;
            return View(usuario);
        }

        public IActionResult ConfirmacaoCompra(int usuarioId, string plano)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Plano = plano;
            ViewBag.Usuario = usuario;
            return View();
        }

        [Authorize]
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Editar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                // 1. Buscar o usuário existente no banco de dados para preservar campos não editáveis
                var usuarioExistente = _context.Usuarios.Find(usuario.Id);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // 2. Atualizar apenas os campos que podem ser editados pelo formulário
                usuarioExistente.Nome = usuario.Nome;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Contato = usuario.Contato;

                // A senha só deve ser atualizada se o campo não estiver vazio
                if (!string.IsNullOrEmpty(usuario.Senha))
                {
                    // Nota: Idealmente, a senha deveria ser hasheada antes de salvar
                    usuarioExistente.Senha = usuario.Senha;
                }

                // Não atualizar CriadorId e IsAdmin, que são definidos na criação

                _context.Usuarios.Update(usuarioExistente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [Authorize]
        public IActionResult Apagar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [Authorize]
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
