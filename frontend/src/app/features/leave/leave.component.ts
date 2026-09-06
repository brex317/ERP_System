import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveService } from '../../core/services/leave.service';
import { LeaveRequestDto } from '../../core/models/leave.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-leave',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  templateUrl: './leave.component.html',
  styleUrls: ['./leave.component.css']
})
export class LeaveComponent implements OnInit {
  requests: LeaveRequestDto[] = [];
  isLoading: boolean = true;

  constructor(private leaveService: LeaveService) {}

  ngOnInit(): void {
    this.loadLeaveRequests();
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

  newRequest(): void {
    alert('Open New Leave Request Form');
  }
}
