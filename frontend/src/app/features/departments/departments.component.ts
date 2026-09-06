import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepartmentService } from '../../core/services/department.service';
import { DepartmentDto } from '../../core/models/department.model';
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

        <app-functionality-help></app-functionality-help>
      </div>

      <div *ngIf="isLoading" class="loading-container">
        <span class="spinner"></span> Loading departments from database...
      </div>

      <div *ngIf="!isLoading" class="table-container">
        <table>
          <thead>
            <tr>
              <th>Code</th>
              <th>Department Name</th>
              <th>Description</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let dept of departments">
              <td><span class="badge">{{ dept.code || 'DEPT' }}</span></td>
              <td><strong>{{ dept.name }}</strong></td>
              <td>{{ dept.description || 'N/A' }}</td>
            </tr>
            <tr *ngIf="departments.length === 0">
              <td colspan="3" class="empty-state">No departments found in database.</td>
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
    .badge {
        padding: 3px 8px;
        background: #eff6ff;
        color: #2563eb;
        border-radius: 6px;
        font-weight: 600;
        font-size: 11px;
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
export class DepartmentsComponent implements OnInit {
  departments: DepartmentDto[] = [];
  isLoading: boolean = true;

  constructor(private departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.isLoading = true;
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load departments:', err);
        this.isLoading = false;
      }
    });
  }

  addDepartment(): void {
    alert('Add Department feature triggered');
  }
}
