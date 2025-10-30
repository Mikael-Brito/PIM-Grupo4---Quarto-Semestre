//<!--
//using Microsoft.AspNetCore.Mvc;
//using MyProject.Data;
//using MyProject.Models;
//using OpenAI;
//using OpenAI.Chat;
//using OpenAI.Models;
//using System.Threading.Tasks;

//namespace MyProject.Controllers
//{
//    public class ChatController : Controller
//    {
//        private readonly AppDbContext _context;
//        private readonly OpenAIClient _openAIClient;

//        public ChatController(AppDbContext context, OpenAIClient openAIClient)
//        {
//            _context = context;
//            _openAIClient = openAIClient;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View(new PerguntaModel());
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(PerguntaModel model)
//        {
//            if (!string.IsNullOrWhiteSpace(model.Pergunta))
//            {
//                model.Resposta = await EnviarParaIA(model.Pergunta);
//                model.Data = DateTime.Now;
//                model.UsuarioId = User?.Identity?.Name ?? "Anônimo";

//                // Salvar no banco (opcional)
//                // _context.Perguntas.Add(model);
//                // await _context.SaveChangesAsync();
//            }

//            return View(model);
//        }

//        private async Task<string> EnviarParaIA(string pergunta)
//        {
//            var chatRequest = new ChatCompletionCreateRequest
//            {
//                Model = Models.Gpt_3_5_Turbo, // ou Models.Gpt_4 se disponível
//                Messages =
//                {
//                    new ChatMessage("user", pergunta)
//                }
//            };

//            var chatResponse = await _openAIClient.ChatCompletions.CreateAsync(chatRequest);

//            return chatResponse.Choices[0].Message.Content;
//        }
//    }
//}
//-->