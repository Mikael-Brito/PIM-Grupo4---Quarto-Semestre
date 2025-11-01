using Microsoft.AspNetCore.Mvc;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class ChatController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Apenas exibe a view vazia
            return View(new PerguntaModel());
        }

        [HttpPost]
        public IActionResult Index(PerguntaModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Pergunta))
            {
                // Apenas uma resposta padrão (sem IA e sem banco)
                model.Resposta = "Função de chat desativada.";
                model.Data = DateTime.Now;
                model.UsuarioId = User?.Identity?.Name ?? "Anônimo";
            }

            return View(model);
        }
    }
}
