import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../core/services/employee.service';
import { EmployeeDto } from '../../core/models/employee.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

export interface PayrollRecord {
  employeeId: number;
  employeeName: string;
  department: string;
  baseSalary: number;
  allowances: number;
  deductions: number;
  netPay: number;
  status: string;
}

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.css']
})
export class PayrollComponent implements OnInit {
  payrolls: PayrollRecord[] = [];
  isLoading: boolean = true;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadPayrollData();
  }

  loadPayrollData(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        this.payrolls = employees.map(e => {
          const baseSalary = 4500 + (e.id * 750);
          const allowances = 500;
          const deductions = baseSalary * 0.15;
          const netPay = baseSalary + allowances - deductions;
          return {
            employeeId: e.id,
            employeeName: `${e.firstName} ${e.lastName}`,
            department: e.departmentName || 'General',
            baseSalary,
            allowances,
            deductions,
            netPay,
            status: 'Processed'
          };
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load payroll data:', err);
        this.isLoading = false;
      }
    });
  }
}
