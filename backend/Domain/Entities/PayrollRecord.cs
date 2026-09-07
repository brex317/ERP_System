using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("payroll")]
public class PayrollRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("pay_period")]
    public string PayPeriod { get; set; } = string.Empty; // e.g. 2026-09

    [Column("base_salary")]
    public decimal BaseSalary { get; set; }

    [Column("allowances")]
    public decimal Allowances { get; set; }

    [Column("deductions")]
    public decimal Deductions { get; set; }

    [Column("net_pay")]
    public decimal NetPay { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Processed"; // Draft, Processed, Paid

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
