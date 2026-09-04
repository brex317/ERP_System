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
}
