using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;
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
            var chamados = _context.Chamados.Include(c => c.Usuario).Include(c => c.Categoria).ToList();
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
            if (ModelState.IsValid)
            {
                chamado.DataAbertura = DateTime.Now;
                _context.Chamados.Add(chamado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(chamado);
        }
        
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
