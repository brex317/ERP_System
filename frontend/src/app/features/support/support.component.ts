import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SupportService } from '../../core/services/support.service';
import { SupportTicketDto, CreateSupportTicketDto } from '../../core/models/support.model';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.css']
})
export class SupportComponent implements OnInit {
  tickets: SupportTicketDto[] = [];
  isLoading: boolean = false;
  isSubmitting: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';

  newTicket: CreateSupportTicketDto = {
    subject: '',
    category: 'General',
    priority: 'Medium',
    message: ''
  };

  categories = ['General', 'Technical', 'Payroll', 'Leave', 'Attendance', 'Account & Security'];
  priorities = ['Low', 'Medium', 'High', 'Urgent'];

  constructor(private supportService: SupportService) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.isLoading = true;
    this.supportService.getTickets().subscribe({
      next: (data) => {
        this.tickets = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading tickets', err);
        this.isLoading = false;
      }
    });
  }

  submitTicket(): void {
    if (!this.newTicket.subject.trim() || !this.newTicket.message.trim()) {
      this.errorMessage = 'Please provide both subject and detailed message.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.supportService.createTicket(this.newTicket).subscribe({
      next: (created) => {
        this.isSubmitting = false;
        this.successMessage = 'Your support ticket has been submitted successfully!';
        this.tickets.unshift(created);
        this.newTicket = { subject: '', category: 'General', priority: 'Medium', message: '' };
        setTimeout(() => this.successMessage = '', 5000);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to submit support ticket. Please try again.';
      }
    });
  }
}
