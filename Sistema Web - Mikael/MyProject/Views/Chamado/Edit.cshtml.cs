using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Views.Chamado
{
    public class EditModel : PageModel
    {
        private readonly MyProject.Data.AppDbContext _context;

        public EditModel(MyProject.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ChamadoModel ChamadoModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamadomodel =  await _context.Chamados.FirstOrDefaultAsync(m => m.Id == id);
            if (chamadomodel == null)
            {
                return NotFound();
            }
            ChamadoModel = chamadomodel;
           ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ChamadoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChamadoModelExists(ChamadoModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ChamadoModelExists(int id)
        {
            return _context.Chamados.Any(e => e.Id == id);
        }
    }
}
