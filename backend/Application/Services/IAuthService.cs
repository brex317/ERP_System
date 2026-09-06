using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IAuthService
{
    Task<LoginResponseDto> AuthenticateAsync(string identifier, string password);
}
