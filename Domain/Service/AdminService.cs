using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Infra.Db;

namespace Minimal.Api.Domain.Service
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        {
            _context = context;
        }
        public Admin? Login(LoginDto loginDto)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
            return admin;
        }
    }
}
