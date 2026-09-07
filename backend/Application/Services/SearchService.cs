using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public class SearchService : ISearchService
{
    private readonly EmsDbContext _db;

    public SearchService(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<GlobalSearchResponseDto> SearchAsync(string query)
    {
        var q = (query ?? string.Empty).Trim().ToLower();
        var results = new List<SearchResultItemDto>();

        if (string.IsNullOrWhiteSpace(q))
        {
            return new GlobalSearchResponseDto { Query = query, Results = results };
        }

        // 1. Search Employees
        var employees = await _db.Employees
            .AsNoTracking()
            .Where(e => e.FirstName.ToLower().Contains(q) ||
                        e.LastName.ToLower().Contains(q) ||
                        e.Email.ToLower().Contains(q) ||
                        (e.Position != null && e.Position.ToLower().Contains(q)))
            .Take(5)
            .ToListAsync();

        foreach (var emp in employees)
        {
            results.Add(new SearchResultItemDto
            {
                Title = $"{emp.FirstName} {emp.LastName}",
                Subtitle = $"{emp.Position ?? "Employee"} • {emp.Email}",
                Category = "Employees",
                LinkUrl = "/employees"
            });
        }

        // 2. Search Departments
        var departments = await _db.Departments
            .AsNoTracking()
            .Where(d => d.Name.ToLower().Contains(q) || d.Code.ToLower().Contains(q))
            .Take(5)
            .ToListAsync();

        foreach (var dept in departments)
        {
            results.Add(new SearchResultItemDto
            {
                Title = dept.Name,
                Subtitle = $"Code: {dept.Code}",
                Category = "Departments",
                LinkUrl = "/departments"
            });
        }

        // 3. Search System Pages/Modules
        var pages = await _db.Pages
            .AsNoTracking()
            .Where(p => p.DisplayName.ToLower().Contains(q) || p.Key.ToLower().Contains(q))
            .Take(5)
            .ToListAsync();

        foreach (var page in pages)
        {
            results.Add(new SearchResultItemDto
            {
                Title = page.DisplayName,
                Subtitle = $"Page in module",
                Category = "Modules",
                LinkUrl = page.RoutePath ?? $"/{page.Key}"
            });
        }

        return new GlobalSearchResponseDto
        {
            Query = query,
            Results = results
        };
    }
}
