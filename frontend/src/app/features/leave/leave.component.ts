import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LeaveService } from '../../core/services/leave.service';
import { EmployeeService } from '../../core/services/employee.service';
import { LeaveRequestDto } from '../../core/models/leave.model';
import { EmployeeDto } from '../../core/models/employee.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-leave',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  templateUrl: './leave.component.html',
  styleUrls: ['./leave.component.css']
})
export class LeaveComponent implements OnInit {
  requests: LeaveRequestDto[] = [];
  employees: EmployeeDto[] = [];
  isLoading: boolean = true;
  statusFilter: string = 'All';

  // Request Modal State
  isModalOpen: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  newRequestData: Partial<LeaveRequestDto> = {
    employeeId: undefined,
    leaveType: 'Annual Leave',
    startDate: new Date().toISOString().substring(0, 10),
    endDate: new Date().toISOString().substring(0, 10),
    reason: ''
  };

  leaveTypes = [
    'Annual Leave',
    'Sick Leave',
    'Maternity Leave',
    'Paternity Leave',
    'Compassionate Leave',
    'Unpaid Leave'
  ];

  constructor(
    private leaveService: LeaveService,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.loadLeaveRequests();
    this.loadEmployees();
  }

  loadLeaveRequests(): void {
    this.isLoading = true;
    this.leaveService.getLeaveRequests().subscribe({
      next: (data) => {
        this.requests = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load leave requests:', err);
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

  get filteredRequests(): LeaveRequestDto[] {
    if (this.statusFilter === 'All') return this.requests;
    return this.requests.filter(r => r.status === this.statusFilter);
  }

  get pendingCount(): number {
    return this.requests.filter(r => r.status === 'Pending').length;
  }

  get approvedCount(): number {
    return this.requests.filter(r => r.status === 'Approved').length;
  }

  get rejectedCount(): number {
    return this.requests.filter(r => r.status === 'Rejected').length;
  }

  openRequestModal(): void {
    this.errorMessage = '';
    this.newRequestData = {
      employeeId: this.employees.length > 0 ? this.employees[0].id : undefined,
      leaveType: 'Annual Leave',
      startDate: new Date().toISOString().substring(0, 10),
      endDate: new Date().toISOString().substring(0, 10),
      reason: ''
    };
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  submitLeaveRequest(): void {
    if (!this.newRequestData.employeeId || !this.newRequestData.reason?.trim()) {
      this.errorMessage = 'Please select an employee and provide a reason.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.leaveService.createLeaveRequest(this.newRequestData).subscribe({
      next: (created) => {
        this.isSubmitting = false;
        this.closeModal();
        this.requests.unshift(created);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to submit leave request.';
      }
    });
  }

  updateStatus(id: number, status: 'Approved' | 'Rejected'): void {
    this.leaveService.updateLeaveStatus(id, status).subscribe({
      next: () => {
        const item = this.requests.find(r => r.id === id);
        if (item) item.status = status;
      },
      error: (err) => console.error('Failed to update leave status', err)
    });
  }
}
