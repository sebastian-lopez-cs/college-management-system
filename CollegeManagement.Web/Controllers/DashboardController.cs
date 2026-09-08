using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagement.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}