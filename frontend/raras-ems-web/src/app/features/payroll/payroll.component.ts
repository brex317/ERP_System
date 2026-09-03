import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FunctionalityHelpComponent],
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.css']
})
export class PayrollComponent {
  payrollHelpSteps = [
    { number: 1, text: 'Open Payroll from the sidebar.' },
    { number: 2, text: 'Review the employee salary information.' },
    { number: 3, text: 'Verify the payroll details before processing.' },
    { number: 4, text: 'Check the calculated payroll information.' },
    { number: 5, text: 'Process payroll according to your organization workflow.' }
  ];
}
