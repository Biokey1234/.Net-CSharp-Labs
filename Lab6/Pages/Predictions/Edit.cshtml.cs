using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Pages.Predictions
{
    public class EditModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;

        public EditModel(Lab6.Data.PredictionDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction =  await _context.Predictions.FirstOrDefaultAsync(m => m.PredictionId == id);
            if (prediction == null)
            {
                return NotFound();
            }
            Prediction = prediction;
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

            _context.Attach(Prediction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredictionExists(Prediction.PredictionId))
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

        private bool PredictionExists(int id)
        {
            return _context.Predictions.Any(e => e.PredictionId == id);
        }
    }
}
