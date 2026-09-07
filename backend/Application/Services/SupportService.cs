using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public class SupportService : ISupportService
{
    private readonly EmsDbContext _db;

    public SupportService(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SupportTicketDto>> GetTicketsAsync()
    {
        var tickets = await _db.SupportTickets
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        if (tickets.Count == 0)
        {
            var initial = new List<SupportTicket>
            {
                new SupportTicket { Subject = "Access Permissions Request", Category = "Account", Priority = "High", Message = "Need HR manager permissions granted for new employee onboarding.", Status = "Resolved", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new SupportTicket { Subject = "Payroll Statement Inquiry", Category = "Payroll", Priority = "Medium", Message = "Question regarding tax deduction calculation on last month payslip.", Status = "Open", CreatedAt = DateTime.UtcNow.AddHours(-10) }
            };

            _db.SupportTickets.AddRange(initial);
            await _db.SaveChangesAsync();
            tickets = initial;
        }

        return tickets.Select(t => new SupportTicketDto
        {
            Id = t.Id,
            Subject = t.Subject,
            Category = t.Category,
            Priority = t.Priority,
            Message = t.Message,
            Status = t.Status,
            CreatedAt = t.CreatedAt
        });
    }

    public async Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto)
    {
        var ticket = new SupportTicket
        {
            Subject = dto.Subject,
            Category = dto.Category,
            Priority = dto.Priority,
            Message = dto.Message,
            Status = "Open",
            CreatedAt = DateTime.UtcNow
        };

        _db.SupportTickets.Add(ticket);
        await _db.SaveChangesAsync();

        return new SupportTicketDto
        {
            Id = ticket.Id,
            Subject = ticket.Subject,
            Category = ticket.Category,
            Priority = ticket.Priority,
            Message = ticket.Message,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt
        };
    }
}
