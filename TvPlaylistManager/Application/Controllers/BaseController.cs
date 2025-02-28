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

        protected IActionResult BaseViewReturn(object? data = null, string? viewName = null)
        {
            if (_notificationHandler.HasNotifications())
                TempData["Notifications"] = GetErrorResponse();

            if (data == null)
                return View(viewName);

            return View(viewName, data);
        }

        protected IActionResult BaseRedirectReturn(string actionName)
        {
            if (_notificationHandler.HasNotifications())
                TempData["Notifications"] = GetErrorResponse();

            return RedirectToAction(actionName);
        }

        private string GetErrorResponse()
        {
            var instance = HttpContext?.Request?.Path.Value;
            var traceId = HttpContext?.TraceIdentifier;

            var response = new ErrorResponse(instance, traceId);

            var errorList = new List<Error>();

            var notifications = _notificationHandler.GetNotifications();

            foreach (var notification in notifications)
            {
                errorList.Add(new Error(notification.Message, notification.Type.ToString()));
            }

            response.Errors = errorList;

            _notificationHandler.Clear();

            var serializadResposne = JsonHelper.Serialize(response);           
            
            return serializadResposne;
        }
    }
}