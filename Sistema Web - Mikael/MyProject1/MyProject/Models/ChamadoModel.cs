using MyProject.Models;

namespace MyProject.Models
{
    public class ChamadoModel
    {
        public int Id { get; set; }

        public required string Titulo { get; set; } = string.Empty;

        public required string Descricao { get; set; } = string.Empty;

        public required string Status { get; set; } = string.Empty;

        public DateTime DataAbertura { get; set; } = DateTime.Now;

        public DateTime? DataFechamento { get; set; } = null;


        public int UsuarioId { get; set; }

        public UsuarioModel? Usuario { get; set; }
    }
}
