import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  template: `
    <div class="page-header">
      <h1>Departments</h1>
      <p>Manage organizational departments.</p>
    </div>

    <div class="card">
      <div class="card-header">
        <h2>Department Management</h2>
        <button class="primary-btn" (click)="addDepartment()">
          + Add Department
        </button>

        <app-functionality-help [steps]="departmentHelpSteps"></app-functionality-help>
      </div>

      <div class="card-body">
        Create, edit and manage organizational departments across your company.
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
    .card-body {
        padding: 20px;
        color: #475569;
        font-size: 14px;
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
  `]
})
export class DepartmentsComponent {
  departmentHelpSteps = [
    { number: 1, text: 'Open Departments from the sidebar.' },
    { number: 2, text: 'Click Add Department.' },
    { number: 3, text: 'Enter the department name and required information.' },
    { number: 4, text: 'Review the department details.' },
    { number: 5, text: 'Save the department.' }
  ];

  addDepartment(): void {
    alert('Open Add Department Form');
  }
}
