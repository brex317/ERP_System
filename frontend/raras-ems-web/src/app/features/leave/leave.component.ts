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

  newRequest(): void {
    alert('Open New Leave Request Form');
  }
}
