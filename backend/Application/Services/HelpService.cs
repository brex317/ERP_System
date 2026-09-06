using Microsoft.Extensions.Caching.Memory;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class HelpService : IHelpService
{
    private readonly IHelpRepository _repository;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public HelpService(IHelpRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<HelpResponseDto> GetHelpAsync(string? moduleKey, string? pageKey, string? functionalityKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim();
        var page = (pageKey ?? string.Empty).Trim();
        var func = (functionalityKey ?? string.Empty).Trim();

        var cacheKey = $"help_context:{mod.ToLowerInvariant()}:{page.ToLowerInvariant()}:{func.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out HelpResponseDto? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        HelpContext? context = null;

        // 1. Try exact functionality-level match if functionalityKey is provided
        if (!string.IsNullOrEmpty(func))
        {
            context = await _repository.FindByFunctionalityAsync(mod, page, func);
        }

        // 2. Fall back to page-level match if not found and pageKey is provided
        if (context == null && !string.IsNullOrEmpty(page))
        {
            context = await _repository.FindByPageAsync(mod, page);
        }

        // 3. Fall back to module-level match if not found and moduleKey is provided
        if (context == null && !string.IsNullOrEmpty(mod))
        {
            context = await _repository.FindByModuleAsync(mod);
        }

        // 4. Additional fallback: If page was provided without module match, try matching by page key directly
        if (context == null && !string.IsNullOrEmpty(page))
        {
            context = await _repository.FindByPageAsync(string.Empty, page);
        }

        // Map to DTO or return safe empty DTO
        HelpResponseDto response;
        if (context != null)
        {
            response = new HelpResponseDto
            {
                ModuleKey = mod,
                PageKey = page,
                FunctionalityKey = func,
                Title = string.IsNullOrWhiteSpace(context.Title) ? "Quick steps" : context.Title,
                Steps = context.Steps
                    .OrderBy(s => s.StepNumber)
                    .Select(s => new HelpStepDto
                    {
                        Number = s.StepNumber,
                        Text = s.StepText
                    })
                    .ToList()
            };
        }
        else
        {
            response = new HelpResponseDto
            {
                ModuleKey = mod,
                PageKey = page,
                FunctionalityKey = func,
                Title = "Quick steps",
                Steps = new List<HelpStepDto>()
            };
        }

        _cache.Set(cacheKey, response, CacheDuration);
        return response;
    }
}
