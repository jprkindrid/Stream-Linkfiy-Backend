using Microsoft.AspNetCore.Mvc;
using Stream_Linkfiy_Backend.Interfaces;

namespace Stream_Linkfiy_Backend.Controllers
{
    [ApiController]
    [Route("/spotify-test")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyTokenService tokenService;
        private readonly ILogger logger;

        public SpotifyController(ISpotifyTokenService tokenService, ILogger logger)
        {
            this.tokenService = tokenService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Token()
        {
            var token = tokenService.GetValidTokenAsync();
            if (token == null)
                return StatusCode(500);

            return Ok(token);
        }
    }
}
