using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<UsuarioModel> Usuarios { get; set; }

        public DbSet<ChamadoModel> Chamados { get; set; }

        public DbSet<CategoriaModel> Categorias { get; set; }
    }
}
