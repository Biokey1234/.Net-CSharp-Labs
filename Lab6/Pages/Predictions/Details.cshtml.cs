using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Pages.Predictions
{
    public class DetailsModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;

        public DetailsModel(Lab6.Data.PredictionDataContext context)
        {
            _context = context;
        }

        public Prediction Prediction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions.FirstOrDefaultAsync(m => m.PredictionId == id);
            if (prediction == null)
            {
                return NotFound();
            }
            else
            {
                Prediction = prediction;
            }
            return Page();
        }
    }
}
