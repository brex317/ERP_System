using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public class NotificationService : INotificationService
{
    private readonly EmsDbContext _db;

    public NotificationService(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync()
    {
        var notifications = await _db.Notifications
            .AsNoTracking()
            .OrderByDescending(n => n.CreatedAt)
            .Take(20)
            .ToListAsync();

        if (notifications.Count == 0)
        {
            // Seed initial notification items if none exist
            var initial = new List<Notification>
            {
                new Notification { Title = "Welcome to RARAS EMS", Message = "System initialized and operational.", Type = "info", CreatedAt = DateTime.UtcNow },
                new Notification { Title = "Leave Request Approved", Message = "Leave request for Tigist Haile has been approved.", Type = "success", CreatedAt = DateTime.UtcNow.AddHours(-2) },
                new Notification { Title = "Payroll Period Ready", Message = "September 2026 payroll statement is ready for review.", Type = "alert", CreatedAt = DateTime.UtcNow.AddHours(-5) }
            };

            _db.Notifications.AddRange(initial);
            await _db.SaveChangesAsync();
            notifications = initial;
        }

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var notification = await _db.Notifications.FindAsync(id);
        if (notification == null) return false;

        notification.IsRead = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task MarkAllAsReadAsync()
    {
        var unread = await _db.Notifications.Where(n => !n.IsRead).ToListAsync();
        foreach (var n in unread)
        {
            n.IsRead = true;
        }
        await _db.SaveChangesAsync();
    }
}
