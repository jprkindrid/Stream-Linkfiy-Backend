using Microsoft.AspNetCore.Mvc;
using Stream_Linkify_Backend.Interfaces.Spotify;
using Stream_Linkify_Backend.Services;

namespace Stream_Linkify_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyTokenService tokenService;
        private readonly ISpotifyTrackService trackService;
        private readonly ILogger<SpotifyController> logger;

        public SpotifyController(
            ISpotifyTokenService tokenService,
            ISpotifyTrackService trackService,
            ILogger<SpotifyController> logger)
        {
            this.tokenService = tokenService;
            this.trackService = trackService;
            this.logger = logger;
        }

        [HttpGet("trackByUrl")]
        public async Task<IActionResult> GetTrack([FromQuery] string spotifyUrl)
        {
            var track = await trackService.GetByUrlAsync(spotifyUrl);

            if (track == null)
                return StatusCode(500);

            return Ok(track);
        }

        [HttpGet("trackByIsrc")]
        public async Task<IActionResult> GetTrackFromIsrc([FromQuery]  string isrc)
        {
            var track = await trackService.GetByIsrcAsync(isrc);

            if (track == null)
                return StatusCode(500);

            return Ok(track);
        }
    }
}
