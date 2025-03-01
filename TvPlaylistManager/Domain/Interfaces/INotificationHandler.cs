using TvPlaylistManager.Domain.Enums;
using TvPlaylistManager.Domain.Models.Notifications;

namespace TvPlaylistManager.Domain.Interfaces
{
    public interface INotificationHandler
    {
        Task Handle(NotificationType notificationType, string notificationMessage);
        bool HasNotifications();
        List<Notification> GetNotifications();
        void Clear();
    }
}
