using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public required string Nome { get; set; }
    }
}
