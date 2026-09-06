import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttendanceService } from '../../core/services/attendance.service';
import { AttendanceDto } from '../../core/models/attendance.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  template: `
    <div class="page-header">
      <h1>Attendance</h1>
      <p>Track and manage employee attendance.</p>
    </div>

    <div class="card">
      <div class="card-header">
        <h2>Today's Attendance</h2>
        <app-functionality-help></app-functionality-help>
      </div>

      <div *ngIf="isLoading" class="loading-container">
        <span class="spinner"></span> Loading attendance records from database...
      </div>

      <div *ngIf="!isLoading" class="table-container">
        <table>
          <thead>
            <tr>
              <th>Employee Name</th>
              <th>Date</th>
              <th>Check In</th>
              <th>Check Out</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let rec of records">
              <td><strong>{{ rec.employeeName || ('Employee #' + rec.employeeId) }}</strong></td>
              <td>{{ rec.date | date:'mediumDate' }}</td>
              <td>{{ rec.checkIn || '-' }}</td>
              <td>{{ rec.checkOut || '-' }}</td>
              <td>
                <span class="status" [ngClass]="rec.status.toLowerCase().replace(' ', '-')">{{ rec.status }}</span>
              </td>
            </tr>
            <tr *ngIf="records.length === 0">
              <td colspan="5" class="empty-state">No attendance records found for today.</td>
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
    .status.late {
        background: #fffbeb;
        color: #b45309;
    }
    .status.on-leave {
        background: #eff6ff;
        color: #1d4ed8;
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
export class AttendanceComponent implements OnInit {
  records: AttendanceDto[] = [];
  isLoading: boolean = true;

  constructor(private attendanceService: AttendanceService) {}

  ngOnInit(): void {
    this.loadAttendance();
  }

  loadAttendance(): void {
    this.isLoading = true;
    this.attendanceService.getAttendance().subscribe({
      next: (data) => {
        this.records = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load attendance:', err);
        this.isLoading = false;
      }
    });
  }
}
