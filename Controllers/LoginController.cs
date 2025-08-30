using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;

namespace Minimal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public LoginController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("admin/login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var result = _adminService.Login(loginDto);
            if (result.Rule == "admin")
                return Ok(new { message = "Login successful" });
            else
                return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
