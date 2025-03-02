using TvPlaylistManager.Domain.Enums;

namespace TvPlaylistManager.Domain.Models.Notifications
{
    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }
    }
}
