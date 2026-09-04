using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelpController : ControllerBase
{
    private readonly EmsDbContext _db;

    public HelpController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<HelpResponseDto>> GetHelp(
        [FromQuery] string? moduleKey,
        [FromQuery] string? pageKey,
        [FromQuery] string? functionalityKey)
    {
        return await FetchHelpData(moduleKey, pageKey, functionalityKey);
    }

    [HttpGet("{moduleKey}/{pageKey}/{functionalityKey}")]
    public async Task<ActionResult<HelpResponseDto>> GetHelpByRoute(
        string moduleKey,
        string pageKey,
        string functionalityKey)
    {
        return await FetchHelpData(moduleKey, pageKey, functionalityKey);
    }

    private async Task<ActionResult<HelpResponseDto>> FetchHelpData(
        string? moduleKey,
        string? pageKey,
        string? functionalityKey)
    {
        var mod = (moduleKey ?? "dashboard").ToLowerTrim();
        var page = (pageKey ?? "overview").ToLowerTrim();
        var func = (functionalityKey ?? "general").ToLowerTrim();

        try
        {
            var context = await _db.HelpContexts
                .Include(c => c.Steps)
                .FirstOrDefaultAsync(c =>
                    c.ModuleKey.ToLower() == mod &&
                    c.PageKey.ToLower() == page &&
                    c.FunctionalityKey.ToLower() == func);

            if (context == null)
            {
                // Fallback check matching moduleKey or return empty structure
                context = await _db.HelpContexts
                    .Include(c => c.Steps)
                    .FirstOrDefaultAsync(c => c.ModuleKey.ToLower() == mod);
            }

            if (context != null)
            {
                return Ok(new HelpResponseDto
                {
                    ModuleKey = context.ModuleKey,
                    PageKey = context.PageKey,
                    FunctionalityKey = context.FunctionalityKey,
                    Title = context.Title,
                    Steps = context.Steps
                        .OrderBy(s => s.StepNumber)
                        .Select(s => new HelpStepDto
                        {
                            Number = s.StepNumber,
                            Text = s.StepText
                        })
                        .ToList()
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HelpController Warning] Database fetch error: {ex.Message}");
        }

        return Ok(new HelpResponseDto
        {
            ModuleKey = mod,
            PageKey = page,
            FunctionalityKey = func,
            Title = "Quick steps",
            Steps = new List<HelpStepDto>()
        });
    }
}

internal static class StringExtensions
{
    public static string ToLowerTrim(this string val) => val.Trim().ToLowerInvariant();
}
