import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PayrollService } from '../../core/services/payroll.service';
import { EmployeeService } from '../../core/services/employee.service';
import { PayrollRecordDto, ProcessPayrollDto } from '../../core/models/payroll.model';
import { EmployeeDto } from '../../core/models/employee.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.css']
})
export class PayrollComponent implements OnInit {
  payrolls: PayrollRecordDto[] = [];
  employees: EmployeeDto[] = [];
  isLoading: boolean = true;
  selectedPeriod: string = '2026-09';

  // Process Modal State
  isProcessModalOpen: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  processData: ProcessPayrollDto = {
    employeeId: 0,
    payPeriod: '2026-09',
    baseSalary: 4500,
    allowances: 500,
    deductions: 450
  };

  // Payslip Modal State
  selectedPayslip: PayrollRecordDto | null = null;

  constructor(
    private payrollService: PayrollService,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.loadPayrollData();
    this.loadEmployees();
  }

  loadPayrollData(): void {
    this.isLoading = true;
    this.payrollService.getPayrollRecords(this.selectedPeriod).subscribe({
      next: (data) => {
        this.payrolls = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load payroll data:', err);
        this.isLoading = false;
      }
    });
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      },
      error: (err) => console.error('Failed to load employees', err)
    });
  }

  get totalBaseSalary(): number {
    return this.payrolls.reduce((sum, p) => sum + p.baseSalary, 0);
  }

  get totalAllowances(): number {
    return this.payrolls.reduce((sum, p) => sum + p.allowances, 0);
  }

  get totalDeductions(): number {
    return this.payrolls.reduce((sum, p) => sum + p.deductions, 0);
  }

  get totalNetSalary(): number {
    return this.payrolls.reduce((sum, p) => sum + p.netSalary, 0);
  }

  openProcessModal(): void {
    this.errorMessage = '';
    this.processData = {
      employeeId: this.employees.length > 0 ? this.employees[0].id : 0,
      payPeriod: this.selectedPeriod,
      baseSalary: 5000,
      allowances: 600,
      deductions: 500
    };
    this.isProcessModalOpen = true;
  }

  closeProcessModal(): void {
    this.isProcessModalOpen = false;
  }

  submitProcessPayroll(): void {
    if (!this.processData.employeeId || this.processData.baseSalary <= 0) {
      this.errorMessage = 'Please select an employee and enter a valid base salary.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.payrollService.processPayroll(this.processData).subscribe({
      next: (created) => {
        this.isSubmitting = false;
        this.closeProcessModal();
        this.loadPayrollData();
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to process payroll record.';
      }
    });
  }

  viewPayslip(item: PayrollRecordDto): void {
    this.selectedPayslip = item;
  }

  closePayslip(): void {
    this.selectedPayslip = null;
  }

  printPayslip(): void {
    window.print();
  }
}
