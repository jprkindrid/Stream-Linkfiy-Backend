using Microsoft.AspNetCore.Mvc;

namespace Stream_Linkify_Backend.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController: ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new {message = "API running. See /scalar for routes"});
        }
    }
}
