using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;
using Minimal.Api.Domain.Interfaces;
using Minimal.Api.Domain.ModelViews;
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

        public Admin Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return admin;
        }

        public List<Admin> GetAll(int? page)
        {
           var query = _context.Admins.AsQueryable();
            if (page.HasValue && page > 0)
            {
                int pageSize = 10;
                int skip = (page.Value - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
            }
            return query.ToList();
        }

        public Admin? GetById(int id)
        {
            var admin = _context.Admins.Find(id);
            return admin;
        }

        public Admin? Login(LoginDto loginDto)
        {
            var adminLogin = _context.Admins.SingleOrDefault(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
            return adminLogin;
        }

        public Admin? Login(AdminDto adminDto)
        {
            throw new NotImplementedException();
        }
    }
}
