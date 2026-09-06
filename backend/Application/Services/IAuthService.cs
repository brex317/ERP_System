using Raras.EMS.API.Controllers;

namespace Raras.EMS.API.Services;

public interface IAuthService
{
    Task<LoginResponseDto> AuthenticateAsync(string identifier, string password);
}
