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
    public class DetailsModel : PageModel
    {
        private readonly MyProject.Data.AppDbContext _context;

        public DetailsModel(MyProject.Data.AppDbContext context)
        {
            _context = context;
        }

        public ChamadoModel ChamadoModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamadomodel = await _context.Chamados.FirstOrDefaultAsync(m => m.Id == id);

            if (chamadomodel is not null)
            {
                ChamadoModel = chamadomodel;

                return Page();
            }

            return NotFound();
        }
    }
}
