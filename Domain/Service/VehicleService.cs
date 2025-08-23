using Microsoft.EntityFrameworkCore;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Infra.Db;

namespace Minimal.Api.Domain.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;
        public VehicleService(AppDbContext context)
        {
            _context = context;
        }

        public Vehicle Create(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return vehicle;
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        public List<Vehicle> GetAll(int page = 1, string? name = null, string? brand = null)
        {
            var query = _context.Vehicles.AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));
            }
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(v => EF.Functions.Like(v.Brand.ToLower(), $"%{brand}%"));
            }
            return query.Skip((page - 1) * 10).Take(10).ToList();
        }

        public Vehicle? GetById(int id)
        {
            return _context.Vehicles.Find(id);
        }

        public Vehicle? Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
            return vehicle;
        }
    }
}
