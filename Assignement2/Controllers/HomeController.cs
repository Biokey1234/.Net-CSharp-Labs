using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
       
            public IActionResult Index()
            {
                ViewData["Message"] = "Welcome to my Lab5 Application!"; 
                return View();
            }

            public IActionResult Error()
            {
                return View();
            }
        }
    
}
