using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelpController : ControllerBase
{
    private readonly IHelpService _helpService;

    public HelpController(IHelpService helpService)
    {
        _helpService = helpService;
    }

    [HttpGet]
    public async Task<ActionResult<HelpResponseDto>> GetHelp(
        [FromQuery] string? moduleKey,
        [FromQuery] string? pageKey,
        [FromQuery] string? functionalityKey)
    {
        var result = await _helpService.GetHelpAsync(moduleKey, pageKey, functionalityKey);
        return Ok(result);
    }

    [HttpGet("{moduleKey}/{pageKey}")]
    public async Task<ActionResult<HelpResponseDto>> GetHelpByModuleAndPage(
        string moduleKey,
        string pageKey)
    {
        var result = await _helpService.GetHelpAsync(moduleKey, pageKey, null);
        return Ok(result);
    }

    [HttpGet("{moduleKey}/{pageKey}/{functionalityKey}")]
    public async Task<ActionResult<HelpResponseDto>> GetHelpByRoute(
        string moduleKey,
        string pageKey,
        string functionalityKey)
    {
        var result = await _helpService.GetHelpAsync(moduleKey, pageKey, functionalityKey);
        return Ok(result);
    }
}
