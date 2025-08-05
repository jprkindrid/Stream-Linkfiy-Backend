using Stream_Linkfiy_Backend.Interfaces;
using Stream_Linkfiy_Backend.Services;

namespace Stream_Linkfiy_Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSpotifyServices(this IServiceCollection services)
        {
            services.AddScoped<ISpotifyTokenService, SpotifyTokenService>();
            services.AddHttpClient<ISpotifyTokenService, SpotifyTokenService>();
            services.AddScoped<ISpotifyTrackService, SpotifyTrackService>();
            services.AddHttpClient<ISpotifyTrackService, SpotifyTrackService>();
            services.AddScoped<ISpotifyAlbumService, SpotifyAlbumService>();
            services.AddHttpClient<ISpotifyAlbumService, SpotifyAlbumService>();
            return services;
        }
    }
}
