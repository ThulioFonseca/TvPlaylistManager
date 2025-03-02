using Microsoft.AspNetCore.Mvc;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.Errors;
using TvPlaylistManager.Infrastructure.Extensions;

namespace TvPlaylistManager.Application.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificationHandler _notificationHandler;

        protected BaseController(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        protected IActionResult BaseViewReturn(object data = null, string viewName = null)
        {
            if (_notificationHandler.HasNotifications())
                TempData["Notifications"] = GetBaseReponse();

            if (data == null)
                return View(viewName);

            return View(viewName, data);
        }

        protected IActionResult BaseRedirectReturn(string actionName)
        {
            if (_notificationHandler.HasNotifications())
                TempData["Notifications"] = GetBaseReponse();

            return RedirectToAction(actionName);
        }

        private string GetBaseReponse()
        {
            var instance = HttpContext?.Request?.Path.Value;
            var traceId = HttpContext?.TraceIdentifier;
            var notifications = _notificationHandler.GetNotifications();

            var response = new BaseResponse(instance, traceId, notifications);       
            var serializadResposne = JsonHelper.Serialize(response);           

            _notificationHandler.Clear();
            
            return serializadResposne;
        }
    }
}