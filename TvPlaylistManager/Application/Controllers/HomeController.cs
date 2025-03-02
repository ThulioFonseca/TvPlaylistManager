using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.Epg;
using TvPlaylistManager.Domain.Models.Errors;

namespace TvPlaylistManager.Application.Controllers;

public class HomeController : BaseController
{
    private readonly IEpgService _epgService;

    public HomeController(IEpgService epgService, INotificationHandler notificationHandler) : base(notificationHandler)
    {
        _epgService = epgService;
    }

    public async Task<IActionResult> Index()
    {
        var epgSources = await _epgService.GetAllEpgSources();

        return BaseViewReturn(epgSources);
    }

    public async Task<IActionResult> Create(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            await _epgService.SaveEpgSource(source);
            return BaseRedirectReturn("Index");
        }

        return BaseViewReturn(source);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);
        return BaseViewReturn(source);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EpgSource source)
    {
        if (ModelState.IsValid)
        {
            await _epgService.UpdateEpgSoure(source);
            return BaseRedirectReturn("Index");
        }

        return BaseViewReturn(source);
    }

    public async Task<IActionResult> Delete(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);

        if (source == null)
        {
            return NotFound();
        }

        return BaseViewReturn(source);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);

        if (source == null)
            return NotFound();

        await _epgService.DeleteEpgSource(id);

        return BaseRedirectReturn("Index");
    }

    public async Task<IActionResult> Details(long id)
    {
        var source = await _epgService.GetEpgSourceById(id);

        if (source == null)
            return NotFound();

        return BaseViewReturn(source);
    }

    public IActionResult Privacy()
    {
        return BaseViewReturn();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
