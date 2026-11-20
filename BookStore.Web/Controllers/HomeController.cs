using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using BookStore.Web.Models;

namespace BookStore.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IActionDescriptorCollectionProvider _actions;

    public HomeController(ILogger<HomeController> logger, IActionDescriptorCollectionProvider actions)
    {
        _logger = logger;
        _actions = actions;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Lab 3 A: /docs/a/b/c → path = "a/b/c"
    public IActionResult Docs(string? path)
    {
        ViewBag.Path = path;
        return View();
    }

    // Lab 3 B1: tabela adres → kontroler.akcja → metoda HTTP
    public IActionResult Routes()
    {
        var rows = _actions.ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .Select(a =>
            {
                a.RouteValues.TryGetValue("area", out var area);
                var address = a.AttributeRouteInfo?.Template is { } template
                    ? "/" + template                                                    // routing atrybutowy
                    : (string.IsNullOrEmpty(area) ? "" : "/" + area) + $"/{a.ControllerName}/{a.ActionName}";
                var methods = a.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                    .SelectMany(c => c.HttpMethods).ToList();
                var method = methods is { Count: > 0 } ? string.Join(", ", methods) : "dowolna";
                return (address, $"{a.ControllerName}.{a.ActionName}", method);
            })
            .OrderBy(r => r.address)
            .ToList();

        return View(rows);
    }

    // Lab 3 E: test filtra wyjątku
    public IActionResult Boom() => throw new InvalidOperationException("Celowy wyjątek");

    // Lab 3 F: strona dla kodów błędów
    public IActionResult HttpStatus(int code)
    {
        Response.StatusCode = code;
        ViewBag.Code = code;
        ViewBag.Description = ReasonPhrases.GetReasonPhrase(code);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
