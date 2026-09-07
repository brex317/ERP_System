using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface ISearchService
{
    Task<GlobalSearchResponseDto> SearchAsync(string query);
}
