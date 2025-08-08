using Microsoft.AspNetCore.Mvc;
using Stream_Linkify_Backend.DTOs;
using Stream_Linkify_Backend.Interfaces.Spotify;
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

        public UrlConversionController(
            ILogger<UrlConversionController> logger,
            ISpotifyInput spotifyInput,
            IAppleInput appleInput
            )
        {
            this.logger = logger;
            this.spotifyInput = spotifyInput;
            this.appleInput = appleInput;
        }

        [HttpPost("tracks")]
        public async Task<IActionResult> ConvertToAllUrls([FromBody] TrackUrlRequestDto request)
        {
            if (!Uri.TryCreate(request.TrackUrl, UriKind.Absolute, out var uri))
                return BadRequest("Invalid URL format");

            try
            {
                TrackModel? resultTrack = uri.Host.ToLower() switch
                {
                    "open.spotify.com" => await spotifyInput.getUrlsAsync(request.TrackUrl),
                    "music.apple.com" => await appleInput.getUrlsAsync(request.TrackUrl),
                    _ => null
                };

                if (resultTrack == null)
                    return NotFound("Track not found");

                return Ok(resultTrack);
            }
            catch (InvalidOperationException ex)
            {
                // Log the error for debugging
                logger.LogWarning(ex, "Track not found or invalid operation for URL: {Url}", request.TrackUrl);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Unexpected error
                logger.LogError(ex, "Unexpected error processing URL: {Url}", request.TrackUrl);
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
