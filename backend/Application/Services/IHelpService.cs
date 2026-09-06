using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IHelpService
{
    Task<HelpResponseDto> GetHelpAsync(string? moduleKey, string? pageKey, string? functionalityKey);
}
