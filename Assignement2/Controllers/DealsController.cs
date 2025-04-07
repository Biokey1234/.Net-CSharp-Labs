using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Controllers
{
    public class DealsController : Controller
    {
        private readonly DealsFinderDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DealsController(DealsFinderDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Deals/Index/{id}
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var service = await _context.FoodDeliveryServices.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var deals = await _context.Deals
                .Where(d => d.FoodDeliveryServiceId == id)
                .ToListAsync();

            var model = new DealsPostsViewModel
            {
                FoodDeliveryService = service,
                Deals = deals
            };

            return View(model);
        }

        // GET: Deals/Create/{id}
        [HttpGet]
        public IActionResult Create(string id)
        {
            var service = _context.FoodDeliveryServices.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            var model = new FileInputViewModel
            {
                FoodDeliveryServiceId = id,
                FoodDeliveryServiceTitle = service.Title
            };

            return View(model);
        }

        // POST: Deals/Create
        [HttpPost]
        public async Task<IActionResult> Create(FileInputViewModel model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a valid file.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            var filePath = Path.Combine(uploadsFolder, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var deal = new Deal
            {
                FoodDeliveryServiceId = model.FoodDeliveryServiceId,
                ImagePath = $"images/{model.File.FileName}"
            };

            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = model.FoodDeliveryServiceId });
        }

        // GET: Deals/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var deal = await _context.Deals
                .Include(d => d.FoodDeliveryService)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deal == null)
            {
                return NotFound();
            }

            return View(deal);
        }

        // POST: Deals/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, deal.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = deal.FoodDeliveryServiceId });
        }
    }
}
