using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TvPlaylistManager.Application.Contracts.Interfaces;
using TvPlaylistManager.Domain.Models;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEpgService _epgService;

    public HomeController(IEpgService epgService)
    {
        _epgService = epgService;
    }

    public async Task<IActionResult> Index()
    {
        var epgSources = await _epgService.GetAllEpgSources();
        return View(epgSources);
    }

    public async Task<IActionResult> Create(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            await _epgService.SaveEpgSource(source);
            return RedirectToAction("Index");
        }
        return View(source);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);
        return View(source);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            await _epgService.UpdateEpgSoure(source);
            return RedirectToAction("Index");
        }

        return View(source);
    }

    public async Task<IActionResult> Delete(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);
        if (source == null)
        {
            return NotFound();
        }

        return View(source);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);

        if (source == null)
            return NotFound();

        await _epgService.DeleteEpgSource(id);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);

        if (source == null)
            return NotFound();

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
