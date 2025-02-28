using TvPlaylistManager.Domain.Models.Notification;

namespace TvPlaylistManager.Domain.Interfaces
{
    public interface INotificationHandler
    {
        Task Handle(Notification notification);
        bool HasNotifications();
        List<Notification> GetNotifications();
        void Clear();
    }
}
