import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-leave',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  templateUrl: './leave.component.html',
  styleUrls: ['./leave.component.css']
})
export class LeaveComponent {
  leaveHelpSteps = [
    { number: 1, text: 'Open Leave Management from the sidebar.' },
    { number: 2, text: 'Click New Request.' },
    { number: 3, text: 'Select the employee and leave type.' },
    { number: 4, text: 'Select the start and end dates.' },
    { number: 5, text: 'Submit the leave request.' }
  ];

  newRequest(): void {
    alert('Open New Leave Request Form');
  }
}
