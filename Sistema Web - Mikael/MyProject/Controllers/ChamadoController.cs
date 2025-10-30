using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;
using System.Linq; // Adicionando o using System.Linq para garantir a compilação.

namespace MyProject.Controllers
{
    public class ChamadoController : Controller
    {
        private readonly AppDbContext _context;
        public ChamadoController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Removendo a obrigatoriedade de login para a visualização, mas mantendo a lógica de filtragem se houver login.
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");
            var isAdmin = HttpContext.Session.GetString("UsuarioLogadoIsAdmin") == "True";

            // Consulta base para incluir dados de usuário e categoria
            var query = _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Categoria)
                .AsQueryable();
            
            if (usuarioIdLogado.HasValue)
            {
                if (isAdmin)
                {
                    // Admin: Vê chamados de todos os usuários que ele criou (incluindo ele mesmo)
                    var usuariosGerenciadosIds = _context.Usuarios
                        .Where(u => u.CriadorId == usuarioIdLogado || u.Id == usuarioIdLogado)
                        .Select(u => u.Id)
                        .ToList();

                    query = query.Where(c => usuariosGerenciadosIds.Contains(c.UsuarioId));
                }
                else
                {
                    // Usuário Normal: Vê apenas os próprios chamados
                    query = query.Where(c => c.UsuarioId == usuarioIdLogado);
                }
            }
            else
            {
                // Sem usuário logado, não mostra nenhum chamado (ou mostra todos, dependendo da regra de negócio. Vou manter a regra de não mostrar se não tem login, mas sem redirecionar)
                // Para não quebrar a tela, vou retornar uma lista vazia.
                query = query.Where(c => false);
            }
            
            var chamados = query.ToList();
            return View(chamados);
        }
        public IActionResult Details(int id)
        {
            var chamado = _context.Chamados.Include(c => c.Usuario).Include(c => c.Categoria).FirstOrDefault(c => c.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }
        public IActionResult Create()
        {
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(ChamadoModel chamado)
        {
            var usuarioIdLogado = HttpContext.Session.GetInt32("UsuarioLogadoId");

            if (!usuarioIdLogado.HasValue)
            {
                // Se não há login, não é possível criar um chamado.
                TempData["Erro"] = "Você precisa estar logado para abrir um chamado.";
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                // Garante que o chamado está vinculado ao usuário logado
                chamado.UsuarioId = usuarioIdLogado.Value;
                chamado.DataAbertura = DateTime.Now;
                _context.Chamados.Add(chamado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(chamado);
        }
        
        //public IActionResult Edit(int id)
        //{
        //    var chamado = _context.Chamados.Include(c => c.Usuario).Include(c => c.Categoria).FirstOrDefault(c => c.Id == id);
        //    if (chamado == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Usuarios = _context.Usuarios.ToList();
        //    ViewBag.Categorias = _context.Categorias.ToList();
        //    return View(chamado);
        //}
        //[HttpPost]
        //public IActionResult Edit(ChamadoModel chamado)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Chamados.Update(chamado);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Usuarios = _context.Usuarios.ToList();
        //    ViewBag.Categorias = _context.Categorias.ToList();
        //    return View(chamado);
        //}

        public IActionResult Editar(int id)
        {
            var chamado = _context.Chamados.Include(c => c.Usuario).Include(c => c.Categoria).FirstOrDefault(c => c.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(chamado);
        }
        [HttpPost]
        public IActionResult Editar(ChamadoModel chamado)
        {
            if (ModelState.IsValid)
            {
                _context.Chamados.Update(chamado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(chamado);
        }

        public IActionResult Fechar(int id)
        {
            var chamado = _context.Chamados.Find(id);
            if (chamado != null)
            {
                chamado.Status = "Fechado";
                chamado.DataFechamento = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var chamado = _context.Chamados.Include(c => c.Usuario).Include(c => c.Categoria).FirstOrDefault(c => c.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var chamado = _context.Chamados.Find(id);
            if (chamado != null)
            {
                _context.Chamados.Remove(chamado);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
