using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Minimal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Home")]
    public class HomeController : ControllerBase
    {
        // Health check endpoint
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy" });
        }

        // Redirect to Swagger UI
        [HttpGet("swagger")]
        public IActionResult Swagger()
        {
            return Redirect("/swagger");
        }
    }
}
