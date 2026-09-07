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

        // 3. Search System Features/Modules
        var features = await _db.Features
            .AsNoTracking()
            .Where(f => f.DisplayName.ToLower().Contains(q) || f.Key.ToLower().Contains(q))
            .Take(5)
            .ToListAsync();

        foreach (var feature in features)
        {
            results.Add(new SearchResultItemDto
            {
                Title = feature.DisplayName,
                Subtitle = $"Feature in module",
                Category = "Modules",
                LinkUrl = feature.RoutePath ?? $"/{feature.Key}"
            });
        }

        return new GlobalSearchResponseDto
        {
            Query = query,
            Results = results
        };
    }
}
