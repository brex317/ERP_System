namespace Raras.EMS.API.Models.DTOs;

public class PayrollDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string PayPeriod { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
    public string Status { get; set; } = "Processed";
    public DateTime CreatedAt { get; set; }
}

public class ProcessPayrollDto
{
    public string PayPeriod { get; set; } = string.Empty;
}
