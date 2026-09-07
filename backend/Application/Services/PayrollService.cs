using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _payrollRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public PayrollService(
        IPayrollRepository payrollRepository,
        IEmployeeRepository employeeRepository)
    {
        _payrollRepository = payrollRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<PayrollDto>> GetPayrollSummaryAsync()
    {
        var records = await _payrollRepository.GetAllAsync();
        if (!records.Any())
        {
            return await ProcessPayrollAsync("2026-09");
        }

        return records.Select(MapToDto);
    }

    public async Task<IEnumerable<PayrollDto>> ProcessPayrollAsync(string payPeriod)
    {
        var period = string.IsNullOrWhiteSpace(payPeriod) ? "2026-09" : payPeriod.Trim();
        var employees = await _employeeRepository.GetAllAsync();

        var newPayrollRecords = new List<PayrollRecord>();

        foreach (var emp in employees)
        {
            decimal baseSalary = 4500 + (emp.Id * 650);
            decimal allowances = 500;
            decimal deductions = baseSalary * 0.15m;
            decimal netPay = baseSalary + allowances - deductions;

            newPayrollRecords.Add(new PayrollRecord
            {
                EmployeeId = emp.Id,
                PayPeriod = period,
                BaseSalary = baseSalary,
                Allowances = allowances,
                Deductions = deductions,
                NetPay = netPay,
                Status = "Processed",
                CreatedAt = DateTime.UtcNow
            });
        }

        var saved = await _payrollRepository.AddRangeAsync(newPayrollRecords);
        var reloaded = await _payrollRepository.GetAllAsync();
        return reloaded.Select(MapToDto);
    }

    private static PayrollDto MapToDto(PayrollRecord p)
    {
        string empName = p.Employee != null ? $"{p.Employee.FirstName} {p.Employee.LastName}".Trim() : $"Employee #{p.EmployeeId}";
        string deptName = p.Employee?.Department != null ? p.Employee.Department.Name : "General";

        return new PayrollDto
        {
            Id = p.Id,
            EmployeeId = p.EmployeeId,
            EmployeeName = empName,
            DepartmentName = deptName,
            PayPeriod = p.PayPeriod,
            BaseSalary = p.BaseSalary,
            Allowances = p.Allowances,
            Deductions = p.Deductions,
            NetPay = p.NetPay,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        };
    }
}
