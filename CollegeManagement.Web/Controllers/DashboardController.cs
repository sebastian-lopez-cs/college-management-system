using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSafety.MVC.Controllers;

[Authorize(Roles = "Administrator")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}