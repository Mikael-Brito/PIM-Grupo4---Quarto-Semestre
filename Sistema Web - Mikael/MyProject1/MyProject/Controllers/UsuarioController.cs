using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

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
