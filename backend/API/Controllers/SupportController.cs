using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly ISupportService _supportService;

    public SupportController(ISupportService supportService)
    {
        _supportService = supportService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupportTicketDto>>> GetTickets()
    {
        var tickets = await _supportService.GetTicketsAsync();
        return Ok(tickets);
    }

    [HttpPost]
    public async Task<ActionResult<SupportTicketDto>> CreateTicket([FromBody] CreateSupportTicketDto dto)
    {
        var created = await _supportService.CreateTicketAsync(dto);
        return Ok(created);
    }
}
