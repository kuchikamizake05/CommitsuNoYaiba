using KlasemenWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace KlasemenWeb.Controllers;

public class KlasemenController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new Klub { Nama = "Persib", Poin = 3 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(Klub klub)
    {
        if (!ModelState.IsValid)
        {
            return View(klub);
        }

        ViewData["Pesan"] = "Data valid. Ini hanya contoh dan tidak disimpan.";
        return View(klub);
    }
}
