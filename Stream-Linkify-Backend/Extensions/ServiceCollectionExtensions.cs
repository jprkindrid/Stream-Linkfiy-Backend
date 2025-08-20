using Stream_Linkify_Backend.Helpers;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Deezer;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Services.Apple;
using Stream_Linkify_Backend.Services.Deezer;
using Stream_Linkify_Backend.Services.Spotify;
using Stream_Linkify_Backend.Services.Tidal;

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
            services.AddScoped<ISpotifyInput, SpotifyInput>();

            return services;
        }

        public static IServiceCollection AddAppleServices(this IServiceCollection services)
        {
            services.AddSingleton<IAppleApiClient, AppleApiClient>();
            services.AddSingleton<IAppleTokenService, AppleTokenService>();
            services.AddScoped<IAppleTrackService, AppleTrackService>();
            services.AddScoped<IAppleAlbumService, AppleAlbumService>();
            services.AddScoped<IAppleInput, AppleInput>();

            return services;
        }

        public static IServiceCollection AddTidalServices(this IServiceCollection services)
        {
            services.AddSingleton<ITidalApiClient, TidalApiClient>();
            services.AddSingleton<ITidalTokenService, TidalTokenService>();
            services.AddScoped<ITidalArtistService, TidalArtistService>();
            services.AddScoped<ITidalTrackService, TidalTrackService>();
            services.AddScoped<ITidalAlbumService, TidalAlbumService>();
            services.AddScoped<ITidalInput, TidalInput>();

            return services;
        }
        public static IServiceCollection AddDeezerServices(this IServiceCollection services)
        {
            services.AddSingleton<IDeezerApiClient, DeezerApiClient>();
            services.AddScoped<IDeezerTrackService, DeezerTrackService>();
            services.AddScoped<IDeezerAlbumService, DeezerAlbumService>();
            services.AddScoped<IDeezerInput, DeezerInput>();

            return services;
        }
    }
}
