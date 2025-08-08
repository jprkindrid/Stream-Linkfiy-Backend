using Stream_Linkfiy_Backend.Helpers;
using Stream_Linkfiy_Backend.Interfaces.Spotify;
using Stream_Linkfiy_Backend.Services;

namespace Stream_Linkfiy_Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSpotifyServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<ISpotifyTokenService, SpotifyTokenService>();
            services.AddScoped<ISpotifyTrackService, SpotifyTrackService>();
            services.AddScoped<ISpotifyAlbumService, SpotifyAlbumService>();

            return services;
        }

        public static IServiceCollection AddAppleServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<AppleTokenGenerator>();

            return services;
        }
    }
}
