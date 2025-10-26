using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Views.Chamado
{
    public class IndexModel : PageModel
    {
        private readonly MyProject.Data.AppDbContext _context;

        public IndexModel(MyProject.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<ChamadoModel> ChamadoModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ChamadoModel = await _context.Chamados
                .Include(c => c.Usuario).ToListAsync();
        }
    }
}
