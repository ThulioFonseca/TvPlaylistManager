using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Services.Epg;
using TvPlaylistManager.Domain.Services.M3U;
using TvPlaylistManager.Infrastructure.Data.Repositories;
using TvPlaylistManager.Infrastructure.Services.Notifications;

namespace TvPlaylistManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IEpgRepository, EpgRepository>();
            services.AddScoped<IM3URepository, M3URepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler, NotificationHandler>();
            services.AddScoped<IEpgService, EpgService>();
            services.AddScoped<IM3UService, M3UService>();

            return services;
        }
    }
}
