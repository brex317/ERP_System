using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
