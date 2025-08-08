using Stream_Linkify_Backend.Helpers;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Services.Apple;
using Stream_Linkify_Backend.Services.Spotify;

namespace Stream_Linkify_Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSpotifyServices(this IServiceCollection services)
        {
            services.AddSingleton<ISpotifyApiClient, SpotifyApiClient>();
            services.AddSingleton<ISpotifyTokenService, SpotifyTokenService>();
            services.AddScoped<ISpotifyTrackService, SpotifyTrackService>();
            services.AddScoped<ISpotifyAlbumService, SpotifyAlbumService>();

            return services;
        }

        public static IServiceCollection AddAppleServices(this IServiceCollection services)
        {
            services.AddSingleton<IAppleApiClient, AppleApiClient>();
            services.AddSingleton<IAppleTokenService, AppleTokenService>();
            services.AddScoped<IAppleTrackService, AppleTrackService>();

            return services;
        }
    }
}
