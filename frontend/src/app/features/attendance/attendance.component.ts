import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AttendanceService } from '../../core/services/attendance.service';
import { EmployeeService } from '../../core/services/employee.service';
import { AttendanceDto } from '../../core/models/attendance.model';
import { EmployeeDto } from '../../core/models/employee.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent implements OnInit {
  records: AttendanceDto[] = [];
  employees: EmployeeDto[] = [];
  isLoading: boolean = true;
  selectedDate: string = new Date().toISOString().substring(0, 10);
  statusFilter: string = 'All';

  // Manual Log Modal State
  isModalOpen: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  newRecord: Partial<AttendanceDto> = {
    employeeId: undefined,
    date: new Date().toISOString(),
    checkIn: '08:30 AM',
    checkOut: '05:00 PM',
    status: 'Present'
  };

  constructor(
    private attendanceService: AttendanceService,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.loadAttendance();
    this.loadEmployees();
  }

  loadAttendance(): void {
    this.isLoading = true;
    this.attendanceService.getAttendance(this.selectedDate).subscribe({
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

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      },
      error: (err) => console.error('Failed to load employees', err)
    });
  }

  get filteredRecords(): AttendanceDto[] {
    return this.records.filter(r => {
      const matchesStatus = this.statusFilter === 'All' || r.status === this.statusFilter;
      return matchesStatus;
    });
  }

  get presentCount(): number {
    return this.records.filter(r => r.status === 'Present').length;
  }

  get lateCount(): number {
    return this.records.filter(r => r.status === 'Late').length;
  }

  get onLeaveCount(): number {
    return this.records.filter(r => r.status === 'On Leave').length;
  }

  openLogModal(): void {
    this.errorMessage = '';
    this.newRecord = {
      employeeId: this.employees.length > 0 ? this.employees[0].id : undefined,
      date: new Date().toISOString(),
      checkIn: '08:30 AM',
      checkOut: '05:00 PM',
      status: 'Present'
    };
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  quickClockIn(): void {
    if (this.employees.length === 0) return;
    const nowTime = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    const payload = {
      employeeId: this.employees[0].id,
      date: new Date().toISOString(),
      checkIn: nowTime,
      checkOut: '-',
      status: 'Present'
    };
    this.attendanceService.logAttendance(payload).subscribe({
      next: (rec) => {
        this.records.unshift(rec);
      },
      error: (err) => console.error('Failed clock in', err)
    });
  }

  saveAttendanceRecord(): void {
    if (!this.newRecord.employeeId) {
      this.errorMessage = 'Please select an employee.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.attendanceService.logAttendance(this.newRecord).subscribe({
      next: (created) => {
        this.isSubmitting = false;
        this.closeModal();
        this.loadAttendance();
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to log attendance record.';
      }
    });
  }
}
