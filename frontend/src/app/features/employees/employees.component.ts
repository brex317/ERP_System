import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../core/services/employee.service';
import { EmployeeDto } from '../../core/models/employee.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  template: `
    <div class="page-header">
      <h1>Employees</h1>
      <p>Manage employee information and records.</p>
    </div>

    <div class="card">
      <div class="card-header">
        <h2>Employee List</h2>
        <button class="primary-btn" (click)="addEmployee()">
          + Add Employee
        </button>

        <app-functionality-help></app-functionality-help>
      </div>

      <div *ngIf="isLoading" class="loading-container">
        <span class="spinner"></span> Loading employees from database...
      </div>

      <div *ngIf="!isLoading" class="table-container">
        <table>
          <thead>
            <tr>
              <th>Employee Name</th>
              <th>Email</th>
              <th>Department</th>
              <th>Position</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let emp of employees">
              <td><strong>{{ emp.firstName }} {{ emp.lastName }}</strong></td>
              <td>{{ emp.email }}</td>
              <td>{{ emp.departmentName || 'General' }}</td>
              <td>{{ emp.position || 'Employee' }}</td>
              <td>
                <span class="status" [ngClass]="emp.status.toLowerCase()">{{ emp.status }}</span>
              </td>
            </tr>
            <tr *ngIf="employees.length === 0">
              <td colspan="5" class="empty-state">No employees found in database.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `,
  styles: [`
    .page-header {
        margin-bottom: 25px;
    }
    .page-header h1 {
        font-size: 26px;
        margin-bottom: 6px;
    }
    .page-header p {
        color: #64748b;
        font-size: 14px;
    }
    .card {
        background: white;
        border: 1px solid #e2e8f0;
        border-radius: 10px;
        margin-bottom: 20px;
        overflow: visible;
    }
    .card-header {
        padding: 18px 20px;
        border-bottom: 1px solid #e2e8f0;
        display: flex;
        justify-content: space-between;
        align-items: center;
        position: relative;
        gap: 12px;
    }
    .card-header h2 {
        font-size: 16px;
    }
    .primary-btn {
        border: none;
        background: #2563eb;
        color: white;
        padding: 9px 15px;
        border-radius: 7px;
        font-size: 13px;
        transition: .2s;
        cursor: pointer;
    }
    .primary-btn:hover {
        background: #1d4ed8;
        transform: translateY(-1px);
    }
    .table-container {
        overflow-x: auto;
    }
    table {
        width: 100%;
        border-collapse: collapse;
    }
    th, td {
        padding: 13px 15px;
        border-bottom: 1px solid #f1f5f9;
        text-align: left;
        font-size: 13px;
    }
    th {
        color: #64748b;
        font-weight: 600;
        background: #f8fafc;
    }
    .status {
        padding: 4px 9px;
        border-radius: 20px;
        font-size: 11px;
        background: #ecfdf5;
        color: #047857;
    }
    .status.inactive {
        background: #fef2f2;
        color: #dc2626;
    }
    .loading-container {
        padding: 30px;
        text-align: center;
        color: #64748b;
        font-size: 14px;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
    }
    .spinner {
        width: 16px;
        height: 16px;
        border: 2px solid #cbd5e1;
        border-top-color: #2563eb;
        border-radius: 50%;
        animation: spin 0.8s linear infinite;
    }
    @keyframes spin {
        to { transform: rotate(360deg); }
    }
    .empty-state {
        text-align: center;
        color: #94a3b8;
        padding: 24px;
    }
  `]
})
export class EmployeesComponent implements OnInit {
  employees: EmployeeDto[] = [];
  isLoading: boolean = true;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load employees:', err);
        this.isLoading = false;
      }
    });
  }

  addEmployee(): void {
    alert('Add Employee feature triggered');
  }
}
