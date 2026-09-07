using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class HelpRepository : IHelpRepository
{
    private readonly EmsDbContext _db;

    public HelpRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<HelpHeader?> FindByFeatureSpecificationAsync(string moduleKey, string featureKey, string featureSpecificationKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();
        var feat = (featureKey ?? string.Empty).Trim().ToLower();
        var spec = (featureSpecificationKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(spec)) return null;

        return await _db.HelpHeaders
            .AsNoTracking()
            .Include(c => c.Details)
            .Include(c => c.FeatureSpecification)
                .ThenInclude(f => f!.Feature)
                    .ThenInclude(p => p!.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.FeatureSpecificationId != null &&
                c.FeatureSpecification != null &&
                c.FeatureSpecification.Key.ToLower() == spec &&
                (string.IsNullOrEmpty(feat) || (c.FeatureSpecification.Feature != null && c.FeatureSpecification.Feature.Key.ToLower() == feat)) &&
                (string.IsNullOrEmpty(mod) || (c.FeatureSpecification.Feature != null && c.FeatureSpecification.Feature.Module != null && c.FeatureSpecification.Feature.Module.Key.ToLower() == mod)));
    }

    public async Task<HelpHeader?> FindByFeatureAsync(string moduleKey, string featureKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();
        var feat = (featureKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(feat)) return null;

        return await _db.HelpHeaders
            .AsNoTracking()
            .Include(c => c.Details)
            .Include(c => c.Feature)
                .ThenInclude(p => p!.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.FeatureId != null &&
                c.Feature != null &&
                c.Feature.Key.ToLower() == feat &&
                (string.IsNullOrEmpty(mod) || (c.Feature.Module != null && c.Feature.Module.Key.ToLower() == mod)));
    }

    public async Task<HelpHeader?> FindByModuleAsync(string moduleKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(mod)) return null;

        return await _db.HelpHeaders
            .AsNoTracking()
            .Include(c => c.Details)
            .Include(c => c.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.ModuleId != null &&
                c.Module != null &&
                c.Module.Key.ToLower() == mod);
    }
}
