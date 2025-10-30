using System;

namespace MyProject.Models
{
    public class PerguntaModel
    {
        public int Id { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public DateTime Data { get; set; }
        public string UsuarioId { get; set; }
    }
}
