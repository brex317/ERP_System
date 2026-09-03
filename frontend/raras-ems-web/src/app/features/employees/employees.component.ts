import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

export interface EmployeeItem {
  id: number;
  name: string;
  department: string;
  position: string;
  status: string;
}

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

        <app-functionality-help [steps]="employeeHelpSteps"></app-functionality-help>
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>Employee</th>
              <th>Department</th>
              <th>Position</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let emp of employees">
              <td>{{ emp.name }}</td>
              <td>{{ emp.department }}</td>
              <td>{{ emp.position }}</td>
              <td>
                <span class="status">{{ emp.status }}</span>
              </td>
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
  `]
})
export class EmployeesComponent {
  employeeHelpSteps = [
    { number: 1, text: 'Open Employees from the sidebar.' },
    { number: 2, text: 'Click Add Employee to open the employee registration form.' },
    { number: 3, text: 'Enter the required personal and employment information.' },
    { number: 4, text: 'Select the employee’s department and position.' },
    { number: 5, text: 'Click Save Employee to complete registration.' }
  ];

  employees: EmployeeItem[] = [
    { id: 1, name: 'John Doe', department: 'IT', position: 'Software Developer', status: 'Active' },
    { id: 2, name: 'Sara Smith', department: 'Finance', position: 'Accountant', status: 'Active' },
    { id: 3, name: 'Abebe Bikila', department: 'Operations', position: 'Operations Manager', status: 'Active' },
    { id: 4, name: 'Tigist Haile', department: 'HR', position: 'HR Specialist', status: 'Active' },
    { id: 5, name: 'Berihu Tadesse', department: 'IT', position: 'System Admin', status: 'Active' }
  ];

  addEmployee(): void {
    alert('Open Add Employee Form');
  }
}
