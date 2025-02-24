using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TvPlaylistManager.Domain.Models;
using TvPlaylistManager.Domain.Models.Epg;
using TvPlaylistManager.Infrastructure.Data;

namespace TvPlaylistManager.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context, ILogger<HomeController> logger)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var epgSources = _context.EpgSources.ToList();
        return View(epgSources);
    }

    public IActionResult Create(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            _context.EpgSources.Add(source);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(source);
    }

    public IActionResult Edit(long id)
    {
        var source = _context.EpgSources.Find(id);
        return View(source);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            _context.EpgSources.Update(source);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(source);
    }

    public IActionResult Delete(long id)
    {
        var source = _context.EpgSources.Find(id);
        if (source == null)
        {
            return NotFound();
        }
        return View(source);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(long id)
    {
        var source = _context.EpgSources.Find(id);

        if (source == null)
        {
            return NotFound();
        }

        _context.EpgSources.Remove(source);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Details(long id)
    {
        var source = _context.EpgSources.Find(id);

        if (source == null) {
            return NotFound();
        }

        return View(source);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
