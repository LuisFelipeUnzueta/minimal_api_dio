using Microsoft.AspNetCore.Mvc;
using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;

namespace Minimal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] string? name = null, [FromQuery] string? brand = null)
        {
            var vehicles = _vehicleService.GetAll(page, name, brand);
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var vehicle = _vehicleService.GetById(id);
            if (vehicle == null)
                return NotFound();
            return Ok(vehicle);
        }

        [HttpPost]
        public IActionResult Create([FromBody] VehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Year = dto.Year
            };
            var created = _vehicleService.Create(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] VehicleDto dto)
        {
            var existing = _vehicleService.GetById(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Brand = dto.Brand;
            existing.Year = dto.Year;

            var updated = _vehicleService.Update(existing);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _vehicleService.GetById(id);
            if (existing == null)
                return NotFound();

            _vehicleService.Delete(existing);
            return NoContent();
        }
    }
}
