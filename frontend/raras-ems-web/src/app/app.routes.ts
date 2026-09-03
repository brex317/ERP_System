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
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
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
        component: DashboardComponent
      },
      {
        path: 'employees',
        component: EmployeesComponent
      },
      {
        path: 'departments',
        component: DepartmentsComponent
      },
      {
        path: 'attendance',
        component: AttendanceComponent
      },
      {
        path: 'leave',
        component: LeaveComponent
      },
      {
        path: 'payroll',
        component: PayrollComponent
      },
      {
        path: 'help',
        component: HelpCenterComponent
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
