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

    public async Task<HelpContext?> FindByFunctionalityAsync(string moduleKey, string pageKey, string functionalityKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();
        var page = (pageKey ?? string.Empty).Trim().ToLower();
        var func = (functionalityKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(func)) return null;

        return await _db.HelpContexts
            .AsNoTracking()
            .Include(c => c.Steps)
            .Include(c => c.Functionality)
                .ThenInclude(f => f!.Page)
                    .ThenInclude(p => p!.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.FunctionalityId != null &&
                c.Functionality != null &&
                c.Functionality.Key.ToLower() == func &&
                (string.IsNullOrEmpty(page) || (c.Functionality.Page != null && c.Functionality.Page.Key.ToLower() == page)) &&
                (string.IsNullOrEmpty(mod) || (c.Functionality.Page != null && c.Functionality.Page.Module != null && c.Functionality.Page.Module.Key.ToLower() == mod)));
    }

    public async Task<HelpContext?> FindByPageAsync(string moduleKey, string pageKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();
        var page = (pageKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(page)) return null;

        return await _db.HelpContexts
            .AsNoTracking()
            .Include(c => c.Steps)
            .Include(c => c.Page)
                .ThenInclude(p => p!.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.PageId != null &&
                c.Page != null &&
                c.Page.Key.ToLower() == page &&
                (string.IsNullOrEmpty(mod) || (c.Page.Module != null && c.Page.Module.Key.ToLower() == mod)));
    }

    public async Task<HelpContext?> FindByModuleAsync(string moduleKey)
    {
        var mod = (moduleKey ?? string.Empty).Trim().ToLower();

        if (string.IsNullOrEmpty(mod)) return null;

        return await _db.HelpContexts
            .AsNoTracking()
            .Include(c => c.Steps)
            .Include(c => c.Module)
            .FirstOrDefaultAsync(c =>
                c.IsActive &&
                c.ModuleId != null &&
                c.Module != null &&
                c.Module.Key.ToLower() == mod);
    }
}
