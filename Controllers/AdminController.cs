using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Enuns;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.ModelViews;
using Swashbuckle.AspNetCore.Annotations;

namespace Minimal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;

        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var validate = new ValidationErrors { Messages = new List<string>() };
            if (string.IsNullOrEmpty(loginDto.Email))
                validate.Messages.Add("Email is required");
            if (string.IsNullOrEmpty(loginDto.Password))
                validate.Messages.Add("Password is required");
            if (validate.Messages.Any())
                return BadRequest(validate);
            var result = _adminService.Login(loginDto);
            if (result != null)
            {
                var token = _authService.GetToken(result);
                var adminResponse = new AdminResponse
                {
                    Id = result.Id,
                    Email = result.Email,
                    Rule = result.Role.ToString()
                };
                return Ok(new { AdminResponse = adminResponse, Token = token });
            }

            else
                return Unauthorized(new { message = "Invalid credentials" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public IActionResult CreateAdmin([FromBody] AdminDto adminDto)
        {
            var validate = new ValidationErrors { Messages = new List<string>() };

            if (string.IsNullOrEmpty(adminDto.Email))
                validate.Messages.Add("Email is required");
            if (string.IsNullOrEmpty(adminDto.Password))
                validate.Messages.Add("Password is required");
            if (string.IsNullOrEmpty(adminDto.Rule))
                validate.Messages.Add("Rule is required");

            if (validate.Messages.Any())
                return BadRequest(validate);

            if (!Enum.TryParse<RoleType>(adminDto.Rule, out var rule))
            {
                validate.Messages.Add("Invalid rule type");
                return BadRequest(validate);
            }

            var admin = new Admin
            {
                Email = adminDto.Email,
                Password = adminDto.Password,
                Role = rule
            };

            var result = _adminService.Add(admin);
            if (result.Role == RoleType.Admin)
                return Ok(new { message = "Admin created successfully" });
            else
                return Unauthorized(new { message = "Invalid credentials" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admins")]
        public IActionResult GetAllAdmins([FromQuery] int? page)
        {
            var admins = _adminService.GetAll(page);
            var adminsResponse = new List<AdminResponse>();

            foreach(var adm in admins)
            {
                adminsResponse.Add(new AdminResponse
                {
                    Id = adm.Id,
                    Email = adm.Email,
                    Rule = adm.Role.ToString()
                });

            }

            return Ok(adminsResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin{id}")]
        public IActionResult GetAdminById(int id)
        {
            var admin = _adminService.GetById(id);

            if (admin == null)
                return NotFound();

            return Ok(admin);
        }
    }
}
