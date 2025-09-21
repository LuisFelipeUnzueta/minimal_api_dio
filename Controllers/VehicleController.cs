using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.ModelViews;
using Swashbuckle.AspNetCore.Annotations;

namespace Minimal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] int? page = 1, [FromQuery] string? name = null, [FromQuery] string? brand = null)
        {
            int validPage = page.HasValue && page.Value > 0 ? page.Value : 1;
            var vehicles = _vehicleService.GetAll(validPage, name, brand);
            return Ok(vehicles);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var vehicle = _vehicleService.GetById(id);
            if (vehicle == null)
                return NotFound();
            return Ok(vehicle);
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] VehicleDto dto)
        {
            var validation = VehicleValidate(dto);
            
            if (validation.Messages.Any())
                return BadRequest(validation);

            var vehicle = new Vehicle
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Year = dto.Year
            };
            var created = _vehicleService.Create(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] VehicleDto dto)
        {
            var existing = _vehicleService.GetById(id);
            if (existing == null)
                return NotFound();

            var validation = VehicleValidate(dto);

            if (validation.Messages.Any())
                return BadRequest(validation);

            existing.Name = dto.Name;
            existing.Brand = dto.Brand;
            existing.Year = dto.Year;

            var updated = _vehicleService.Update(existing);
            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _vehicleService.GetById(id);
            if (existing == null)
                return NotFound();

            _vehicleService.Delete(existing);
            return NoContent();
        }

        private ValidationErrors VehicleValidate(VehicleDto vehicleDto)
        {
            var validation = new ValidationErrors();

            if (string.IsNullOrWhiteSpace(vehicleDto.Name))
                validation.Messages.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(vehicleDto.Brand))
                validation.Messages.Add("Brand is required.");
            if (vehicleDto.Year <= 1900)
                validation.Messages.Add("Year must be greater than 1900.");

            return validation;
        }
    }
}
