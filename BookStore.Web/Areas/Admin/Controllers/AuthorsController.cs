using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers;

// Lab 3 C1: lista + dodawanie autora
[Area("Admin")]
public class AuthorsController : Controller
{
    public IActionResult Index() => View(AuthorStoreMemory.Authors);

    [HttpGet]
    public IActionResult Create() => View(new Author());

    [HttpPost]
    public IActionResult Create(Author model)
    {
        if (!ModelState.IsValid) return View(model);

        model.Id = AuthorStoreMemory.Authors.Max(a => a.Id) + 1;
        AuthorStoreMemory.Authors.Add(model);
        return RedirectToAction(nameof(Index));
    }
}
