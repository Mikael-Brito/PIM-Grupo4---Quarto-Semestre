using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public required string Nome { get; set; } = string.Empty;

        public required string Email { get; set; } = string.Empty;

        public required string Senha { get; set; } = string.Empty;

        public required string Contato { get; set; } = string.Empty;

    }
}
