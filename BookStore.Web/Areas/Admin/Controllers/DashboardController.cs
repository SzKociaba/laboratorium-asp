using BookStore.Web.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly RequestStats _stats;
    public DashboardController(RequestStats stats) => _stats = stats;

    public IActionResult Index()
    {
        ViewBag.Total = _stats.Total;
        return View(_stats.Top(5).ToList());   // zadanie 2: 5 najpopularniejszych ścieżek
    }
}
