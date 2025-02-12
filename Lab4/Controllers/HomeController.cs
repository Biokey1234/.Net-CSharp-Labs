using Lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SongForm() => View();

        [HttpPost]
        public IActionResult Sing(int monkeys)
        {
            if (monkeys < 20 || monkeys > 30)
            {
                return RedirectToAction("SongForm");
            }

            ViewData["MonkeyCount"] = monkeys;
            return View();
        }

        public IActionResult CreateStudent() => View();
        
        [HttpPost]
        public IActionResult DisplayStudent(Student student)
        {
            return View(student); 
        }
        
        public IActionResult Error()
        {
            return View();
        }

    }
}
