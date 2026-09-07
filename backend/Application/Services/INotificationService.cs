using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync();
    Task<bool> MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync();
}
