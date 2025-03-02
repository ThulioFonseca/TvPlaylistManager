using TvPlaylistManager.Domain.Models.Notifications;

namespace TvPlaylistManager.Domain.Models.Errors
{
    public class BaseResponse
    {
        public string Instance { get; set; }
        public string TraceId { get; set; }
        public List<Notification> Notifications { get; set; }
        public BaseResponse(string instance, string traceId, List<Notification> notifications)
        {
            Notifications = notifications;
            Instance = instance;
            TraceId = traceId;
        }
    }
}
