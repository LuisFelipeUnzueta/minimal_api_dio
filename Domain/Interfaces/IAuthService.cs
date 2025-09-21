using Minimal.Api.Domain.Entity;

namespace Minimal.Api.Domain.Interfaces
{
    public interface IAuthService
    {
        string GetToken(Admin admin);
    }
}
