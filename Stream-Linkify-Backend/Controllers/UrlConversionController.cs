using Microsoft.AspNetCore.Mvc;
using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Interfaces.Apple;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Interfaces.Tidal;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlConversionController : ControllerBase
    {
        private readonly ILogger<UrlConversionController> logger;
        private readonly ISpotifyInput spotifyInput;
        private readonly IAppleInput appleInput;
        private readonly ITidalInput tidalInput;

        public UrlConversionController(
            ILogger<UrlConversionController> logger,
            ISpotifyInput spotifyInput,
            IAppleInput appleInput,
            ITidalInput tidalInput
            )
        {
            this.logger = logger;
            this.spotifyInput = spotifyInput;
            this.appleInput = appleInput;
            this.tidalInput = tidalInput;
        }

        [HttpPost("tracks")]
        public async Task<IActionResult> ConvertToAllUrls([FromBody] TrackUrlRequestDto request)
        {
            if (!Uri.TryCreate(request.TrackUrl, UriKind.Absolute, out var uri))
                return BadRequest("Invalid URL format");

            try
            {
                TrackReturnDto? resultTrack = uri.Host.ToLowerInvariant() switch
                {
                    "open.spotify.com" => await spotifyInput.GetUrlsAsync(request.TrackUrl),
                    "music.apple.com" => await appleInput.GetUrlsAsync(request.TrackUrl),
                    "listen.tidal.com" or "tidal.com" => await tidalInput.GetUrlsAsync(request.TrackUrl),
                    _ => null
                };

                if (resultTrack == null)
                    return NotFound("Track not found");

                return Ok(resultTrack);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "Track not found or invalid operation for URL: {Url}", request.TrackUrl);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error processing URL: {Url}", request.TrackUrl);
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
