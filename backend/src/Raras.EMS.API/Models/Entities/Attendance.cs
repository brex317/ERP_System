using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("attendance")]
public class Attendance
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Present";

    [Column("check_in")]
    public TimeSpan? CheckIn { get; set; }

    [Column("check_out")]
    public TimeSpan? CheckOut { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
