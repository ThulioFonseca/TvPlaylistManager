using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Services.Epg;
using TvPlaylistManager.Infrastructure.Data.Repositories;

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

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEpgService, EpgService>();

            return services;
        }
    }
}
