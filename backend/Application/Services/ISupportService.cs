using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface ISupportService
{
    Task<IEnumerable<SupportTicketDto>> GetTicketsAsync();
    Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto);
}
