using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Views.Chamado
{
    public class CreateModel : PageModel
    {
        private readonly MyProject.Data.AppDbContext _context;

        public CreateModel(MyProject.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ChamadoModel ChamadoModel { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Chamados.Add(ChamadoModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Criar");
        }
    }
}
