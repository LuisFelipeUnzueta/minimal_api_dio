using Minimal.Api.Domain.Dto;
using Minimal.Api.Domain.Entity;

namespace Minimal.Api.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDto loginDto);

        Admin Add(Admin admin);

        List<Admin> GetAll(int? page);

        Admin? GetById(int id);
    }
}
