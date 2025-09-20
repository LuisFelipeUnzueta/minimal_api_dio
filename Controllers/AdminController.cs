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

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

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

            if (!Enum.TryParse<RuleType>(adminDto.Rule, out var rule))
            {
                validate.Messages.Add("Invalid rule type");
                return BadRequest(validate);
            }

            var admin = new Admin
            {
                Email = adminDto.Email,
                Password = adminDto.Password,
                Rule = rule
            };

            var result = _adminService.Add(admin);
            if (result.Rule == RuleType.Admin)
                return Ok(new { message = "Admin created successfully" });
            else
                return Unauthorized(new { message = "Invalid credentials" });
        }

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
                    Rule = adm.Rule.ToString()
                });

            }

            return Ok(adminsResponse);
        }

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
