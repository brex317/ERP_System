import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardService } from '../../core/services/dashboard.service';
import { DashboardStats } from '../../core/models/dashboard-stats.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FunctionalityHelpComponent],
  template: `
    <div class="page-header">
      <h1>Dashboard</h1>
      <p>Overview of your employee management system.</p>
    </div>

    <div class="stats">
      <div class="stat-card">
        <small>Total Employees</small>
        <strong id="stat-total-employees">{{ stats.totalEmployees }}</strong>
      </div>

      <div class="stat-card">
        <small>Departments</small>
        <strong id="stat-departments">{{ stats.totalDepartments }}</strong>
      </div>

      <div class="stat-card">
        <small>Present Today</small>
        <strong id="stat-present-today">{{ stats.presentToday }}</strong>
      </div>

      <div class="stat-card">
        <small>On Leave</small>
        <strong id="stat-on-leave">{{ stats.onLeave }}</strong>
      </div>
    </div>

    <div class="card">
      <div class="card-header">
        <h2>Welcome to RARAS EMS</h2>
        <button class="primary-btn" routerLink="/employees">
          View Employees →
        </button>

        <app-functionality-help moduleKey="dashboard" pageKey="overview" functionalityKey="general"></app-functionality-help>
      </div>

      <div class="card-body">
        Manage employees, departments, attendance, leave and payroll from one centralized system.
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
    .stats {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 16px;
        margin-bottom: 25px;
    }
    .stat-card {
        background: white;
        border: 1px solid #e2e8f0;
        border-radius: 10px;
        padding: 20px;
        transition: .2s;
    }
    .stat-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 8px 20px rgba(15, 23, 42, .06);
    }
    .stat-card small {
        color: #64748b;
        font-size: 12px;
    }
    .stat-card strong {
        display: block;
        font-size: 25px;
        margin-top: 8px;
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
    @media (max-width: 1050px) {
        .stats {
            grid-template-columns: repeat(2, 1fr);
        }
    }
    @media (max-width: 700px) {
        .stats {
            grid-template-columns: 1fr;
        }
    }
  `]
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats = {
    totalEmployees: 248,
    totalDepartments: 12,
    presentToday: 221,
    onLeave: 18
  };

  isLoading: boolean = true;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.dashboardService.getDashboardStats().subscribe({
      next: (data) => {
        this.stats = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load dashboard stats:', err);
        this.isLoading = false;
      }
    });
  }
}
