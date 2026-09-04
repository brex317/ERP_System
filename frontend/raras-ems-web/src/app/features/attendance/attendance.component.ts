import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
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
        <app-functionality-help moduleKey="attendance" pageKey="attendance-list" functionalityKey="manage-attendance"></app-functionality-help>
      </div>

      <div class="card-body">
        Attendance records and check-in times will appear here.
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
  `]
})
export class AttendanceComponent {
}
