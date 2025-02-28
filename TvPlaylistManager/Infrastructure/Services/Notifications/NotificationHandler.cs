using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.Notification;

namespace TvPlaylistManager.Infrastructure.Services.Notifications
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly List<Notification> _notifications;

        public NotificationHandler()
        {
            _notifications = [];
        }

        public void Clear()
        {
            _notifications.Clear();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public Task Handle(Notification notification)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public bool HasNotifications()
        {
            return _notifications.Count > 0;
        }
    }
}
