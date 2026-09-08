using System.Diagnostics;
using FoodSafety.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FoodSafety.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Log.Information("Home page accessed by {User}. Redirecting to Dashboard.", User?.Identity?.Name ?? "Anonymous");
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Privacy()
        {
            Log.Information("Privacy page viewed by {User}.", User?.Identity?.Name ?? "Anonymous");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            Log.Error("Error page shown. RequestId={RequestId}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}