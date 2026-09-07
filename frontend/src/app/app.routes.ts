import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { EmployeesComponent } from './features/employees/employees.component';
import { DepartmentsComponent } from './features/departments/departments.component';
import { AttendanceComponent } from './features/attendance/attendance.component';
import { LeaveComponent } from './features/leave/leave.component';
import { PayrollComponent } from './features/payroll/payroll.component';
import { HelpCenterComponent } from './features/help/help-center.component';
import { SupportComponent } from './features/support/support.component';
import { SettingsComponent } from './features/settings/settings.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    data: { module: 'auth', page: 'login', functionality: 'login-form' }
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
        data: { module: 'dashboard', page: 'overview' }
      },
      {
        path: 'employees',
        component: EmployeesComponent,
        data: { module: 'employees', page: 'employee-list' }
      },
      {
        path: 'departments',
        component: DepartmentsComponent,
        data: { module: 'departments', page: 'department-list' }
      },
      {
        path: 'attendance',
        component: AttendanceComponent,
        data: { module: 'attendance', page: 'attendance-list' }
      },
      {
        path: 'leave',
        component: LeaveComponent,
        data: { module: 'leave', page: 'leave-list' }
      },
      {
        path: 'payroll',
        component: PayrollComponent,
        data: { module: 'payroll', page: 'payroll-list' }
      },
      {
        path: 'help',
        component: HelpCenterComponent,
        data: { module: 'help', page: 'help-center' }
      },
      {
        path: 'support',
        component: SupportComponent,
        data: { module: 'support', page: 'contact-support' }
      },
      {
        path: 'settings',
        component: SettingsComponent,
        data: { module: 'settings', page: 'user-settings' }
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
