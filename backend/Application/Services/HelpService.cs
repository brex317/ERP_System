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
        var feat = (pageKey ?? string.Empty).Trim();
        var spec = (functionalityKey ?? string.Empty).Trim();

        var cacheKey = $"help_context:{mod.ToLowerInvariant()}:{feat.ToLowerInvariant()}:{spec.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out HelpResponseDto? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        HelpHeader? header = null;

        // 1. Try exact feature-specification-level match if functionalityKey/featureSpecificationKey is provided
        if (!string.IsNullOrEmpty(spec))
        {
            header = await _repository.FindByFeatureSpecificationAsync(mod, feat, spec);
        }

        // 2. Fall back to feature-level match if not found and pageKey/featureKey is provided
        if (header == null && !string.IsNullOrEmpty(feat))
        {
            header = await _repository.FindByFeatureAsync(mod, feat);
        }

        // 3. Fall back to module-level match if not found and moduleKey is provided
        if (header == null && !string.IsNullOrEmpty(mod))
        {
            header = await _repository.FindByModuleAsync(mod);
        }

        // 4. Additional fallback: If feature key was provided without module match, try matching by feature key directly
        if (header == null && !string.IsNullOrEmpty(feat))
        {
            header = await _repository.FindByFeatureAsync(string.Empty, feat);
        }

        // Map to DTO or return safe empty DTO
        HelpResponseDto response;
        if (header != null)
        {
            response = new HelpResponseDto
            {
                ModuleKey = mod,
                FeatureKey = feat,
                FeatureSpecificationKey = spec,
                Title = string.IsNullOrWhiteSpace(header.Title) ? "Quick steps" : header.Title,
                Steps = header.Details
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
                FeatureKey = feat,
                FeatureSpecificationKey = spec,
                Title = "Quick steps",
                Steps = new List<HelpStepDto>()
            };
        }

        _cache.Set(cacheKey, response, CacheDuration);
        return response;
    }
}
