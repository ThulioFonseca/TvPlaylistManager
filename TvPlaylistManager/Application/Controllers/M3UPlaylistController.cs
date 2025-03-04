using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Application.Controllers
{
    public class M3UPlaylistController : BaseController
    {
        private readonly IM3UService _m3uService;
        private readonly IEpgService _epgService;

        public M3UPlaylistController(IM3UService m3UService, IEpgService epgService, INotificationHandler notificationHandler) : base(notificationHandler)
        {
            _m3uService = m3UService;
            _epgService = epgService;
        }

        // GET: M3UPlaylist
        public async Task<IActionResult> Index()
        {
            var m3uPlaylists = await _m3uService.GetAllM3uPlaylits();
            return BaseViewReturn(m3uPlaylists);
        }

        // GET: M3UPlaylist/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var m3uPlaylist = await _m3uService.GetM3uPlaylistById(id);

            if (m3uPlaylist == null)
                return NotFound();

            return BaseViewReturn(m3uPlaylist);
        }

        // GET: M3UPlaylist/Create
        public async Task<IActionResult> Create()
        {
            ViewData["EpgSourceId"] = new SelectList(await _epgService.GetAllEpgSources(), "Id", "Alias");
            return View();
        }

        // POST: M3UPlaylist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(M3UPlaylist m3UPlaylist)
        {
            if (ModelState.IsValid)
            {
                await _m3uService.SaveM3uPlaylist(m3UPlaylist);
                return BaseRedirectReturn("Index");
            }
            ViewData["EpgSourceId"] = new SelectList(await _epgService.GetAllEpgSources(), "Id", "Alias");
            return BaseViewReturn(m3UPlaylist);
        }

        // GET: M3UPlaylist/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var m3uPlaylist = await _m3uService.GetM3uPlaylistById(id);
            ViewData["EpgSourceId"] = new SelectList(await _epgService.GetAllEpgSources(), "Id", "Alias");
            return BaseViewReturn(m3uPlaylist);
        }

        // POST: M3UPlaylist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Url,CreatedAt,UpdatedAt,EpgSourceId")] M3UPlaylist m3UPlaylist)
        {
            if (ModelState.IsValid)
            {
                await _m3uService.UpdateM3uPlaylist(m3UPlaylist);
                return BaseRedirectReturn("Index");
            }

            ViewData["EpgSourceId"] = new SelectList(await _epgService.GetAllEpgSources(), "Id", "Alias");
            return BaseViewReturn(m3UPlaylist);
        }

        // GET: M3UPlaylist/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var m3UPlaylist = await _m3uService.GetM3uPlaylistById(id);

            if (m3UPlaylist == null)
            {
                return NotFound();
            }

            return BaseViewReturn(m3UPlaylist);
        }

        // POST: M3UPlaylist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var m3UPlaylist = await _m3uService.GetM3uPlaylistById(id);

            if (m3UPlaylist == null)
                return NotFound();

            await _m3uService.DeleteM3uPlaylist(id);

            return BaseRedirectReturn("Index");
        }

    }
}
