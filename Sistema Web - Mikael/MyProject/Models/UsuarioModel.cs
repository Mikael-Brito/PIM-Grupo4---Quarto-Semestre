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
        
        // Campo para rastrear o criador (necessário para a lógica de visualização)
        public int? CriadorId { get; set; }
        
        // Campo para diferenciar administradores (apenas o primeiro usuário criado será Admin por empresa)
        public bool IsAdmin { get; set; } = false;



    }
}
