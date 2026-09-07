import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

export interface GuideArticle {
  id: number;
  category: string;
  title: string;
  summary: string;
  icon: string;
  steps: string[];
}

@Component({
  selector: 'app-help-center',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './help-center.component.html',
  styleUrls: ['./help-center.component.css']
})
export class HelpCenterComponent implements OnInit {
  searchQuery: string = '';
  selectedCategory: string = 'All';
  selectedArticle: GuideArticle | null = null;

  categories = [
    { key: 'All', icon: '🌟', label: 'All Topics' },
    { key: 'Getting Started', icon: '🚀', label: 'Getting Started' },
    { key: 'Employees', icon: '👥', label: 'Employee Management' },
    { key: 'Departments', icon: '🏢', label: 'Departments' },
    { key: 'Attendance', icon: '📅', label: 'Attendance & Time' },
    { key: 'Leave', icon: '🏖', label: 'Leave Requests' },
    { key: 'Payroll', icon: '💰', label: 'Payroll & Salaries' }
  ];

  articles: GuideArticle[] = [
    {
      id: 1,
      category: 'Getting Started',
      title: 'Navigating the RARAS EMS Dashboard',
      summary: 'Learn how to quickly locate key metrics, pending leave approvals, and quick actions on your main dashboard.',
      icon: '🚀',
      steps: [
        'Log in with your credentials to land on the main Dashboard.',
        'View live statistics including total employees, present count, pending leaves, and active departments.',
        'Use the top Global Search bar to instantly locate employees or modules.',
        'Access quick actions or notification panel from the top header navigation bar.'
      ]
    },
    {
      id: 2,
      category: 'Employees',
      title: 'How to Add a New Employee',
      summary: 'Complete guide on registering new staff members, assigning departments, and setting up initial employee profiles.',
      icon: '👥',
      steps: [
        'Navigate to the Employees module from the sidebar menu.',
        'Click the "+ Add Employee" button in the upper right header.',
        'Fill in First Name, Last Name, Email, Department, Position, and Salary details.',
        'Click "Save Employee" to insert the record directly into the database.'
      ]
    },
    {
      id: 3,
      category: 'Departments',
      title: 'Creating and Managing Departments',
      summary: 'Set up organizational units, department codes, descriptions, and view associated employee counts.',
      icon: '🏢',
      steps: [
        'Go to the Departments section in the navigation menu.',
        'Click "+ Add Department" to bring up the department modal.',
        'Enter Department Name, Department Code (e.g. IT, HR, FIN), and optional description.',
        'Save the department and assign employees to it.'
      ]
    },
    {
      id: 4,
      category: 'Attendance',
      title: 'Daily Attendance Check-In and Check-Out',
      summary: 'Record daily attendance records, monitor check-in times, and generate attendance logs.',
      icon: '📅',
      steps: [
        'Navigate to the Attendance page.',
        'Click "Clock In" when starting your shift.',
        'The system records your check-in timestamp and status.',
        'At the end of your shift, click "Clock Out" to complete your daily log.'
      ]
    },
    {
      id: 5,
      category: 'Leave',
      title: 'Submitting and Approving Leave Requests',
      summary: 'Learn how employees request time off and how supervisors review and approve/reject applications.',
      icon: '🏖',
      steps: [
        'Open the Leave Management section.',
        'Click "Request Leave" and choose the Leave Type (Annual, Sick, Maternity, Unpaid).',
        'Select Start Date, End Date, and enter a reason.',
        'Administrators review pending requests and click Approve or Reject.'
      ]
    },
    {
      id: 6,
      category: 'Payroll',
      title: 'Processing Payroll and Viewing Payslips',
      summary: 'Calculate gross salaries, deductions, allowances, net pay, and generate monthly payslips.',
      icon: '💰',
      steps: [
        'Navigate to the Payroll section.',
        'Select the target Pay Period (e.g. September 2026).',
        'Click "Process Payroll" to calculate base salaries, allowances, and deductions.',
        'Click "View Payslip" on any record to view or print detailed salary breakdown.'
      ]
    }
  ];

  ngOnInit(): void {}

  get filteredArticles(): GuideArticle[] {
    return this.articles.filter(a => {
      const matchesCat = this.selectedCategory === 'All' || a.category === this.selectedCategory;
      const matchesSearch = !this.searchQuery.trim() ||
        a.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        a.summary.toLowerCase().includes(this.searchQuery.toLowerCase());
      return matchesCat && matchesSearch;
    });
  }

  selectCategory(catKey: string): void {
    this.selectedCategory = catKey;
  }

  openArticle(article: GuideArticle): void {
    this.selectedArticle = article;
  }

  closeModal(): void {
    this.selectedArticle = null;
  }
}
