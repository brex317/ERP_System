import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface HelpCategory {
  icon: string;
  title: string;
  count: string;
  description: string;
}

@Component({
  selector: 'app-help-center',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './help-center.component.html',
  styleUrls: ['./help-center.component.css']
})
export class HelpCenterComponent {
  searchQuery: string = '';

  categories: HelpCategory[] = [
    { icon: '🚀', title: 'Getting Started', count: '4 guides', description: 'Learn the basics of using the employee management system.' },
    { icon: '👤', title: 'User & Account', count: '5 guides', description: 'Manage users, roles, permissions and accounts.' },
    { icon: '🏢', title: 'Organization Management', count: '3 guides', description: 'Manage departments, structure and hierarchy.' },
    { icon: '👥', title: 'Employee Management', count: '8 guides', description: 'Add, edit, search and manage employee records.' },
    { icon: '📅', title: 'Attendance & Leave', count: '7 guides', description: 'Record attendance and manage employee leave.' },
    { icon: '💰', title: 'Payroll', count: '6 guides', description: 'Manage salary information and payroll processing.' }
  ];
}
