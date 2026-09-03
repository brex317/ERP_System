import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface HelpStep {
  number: number;
  text: string;
}

@Component({
  selector: 'app-functionality-help',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="functionality-help">
      <span class="functionality-help-trigger" tabindex="0" aria-label="Need help">ⓘ Need help?</span>
      <div class="functionality-help-menu">
        <div class="functionality-help-title">{{ title }}</div>
        <div *ngFor="let step of steps" class="functionality-help-step">
          <span class="functionality-help-number">{{ step.number }}</span>
          <span>{{ step.text }}</span>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .functionality-help {
        position: relative;
        margin-left: auto;
        flex: 0 0 auto;
        align-self: center;
        z-index: 20;
    }
    .functionality-help-trigger {
        display: inline-flex;
        align-items: center;
        border: 0;
        background: transparent;
        color: #2563eb;
        padding: 5px 0;
        margin: 0;
        font-size: 12px;
        font-weight: 600;
        line-height: 1.4;
        white-space: nowrap;
        cursor: default;
        text-decoration: none;
    }
    .functionality-help:hover .functionality-help-trigger,
    .functionality-help:focus-within .functionality-help-trigger {
        color: #1d4ed8;
    }
    .functionality-help-menu {
        position: absolute;
        top: calc(100% + 8px);
        right: 0;
        width: min(340px, calc(100vw - 32px));
        max-width: 340px;
        background: #fff;
        border: 1px solid #dbe3ef;
        border-radius: 10px;
        box-shadow: 0 14px 32px rgba(15,23,42,.14);
        padding: 14px 16px;
        visibility: hidden;
        opacity: 0;
        transform: translateY(-5px);
        pointer-events: none;
        transition: opacity .16s ease, transform .16s ease, visibility .16s ease;
        z-index: 9999;
    }
    .functionality-help:hover .functionality-help-menu,
    .functionality-help:focus-within .functionality-help-menu {
        visibility: visible;
        opacity: 1;
        transform: translateY(0);
        pointer-events: auto;
    }
    .functionality-help-title {
        color: #334155;
        font-size: 13px;
        font-weight: 700;
        margin-bottom: 7px;
    }
    .functionality-help-step {
        display: flex;
        align-items: flex-start;
        gap: 9px;
        padding: 8px 0;
        border-bottom: 1px solid #f1f5f9;
        color: #64748b;
        font-size: 12px;
        line-height: 1.5;
    }
    .functionality-help-step:last-child {
        border-bottom: 0;
        padding-bottom: 0;
    }
    .functionality-help-number {
        width: 21px;
        height: 21px;
        flex: 0 0 21px;
        border-radius: 50%;
        background: #eff6ff;
        color: #2563eb;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 10px;
        font-weight: 700;
    }
  `]
})
export class FunctionalityHelpComponent {
  @Input() title: string = 'Quick steps';
  @Input() steps: HelpStep[] = [
    { number: 1, text: 'Use the sidebar to open the module you need.' },
    { number: 2, text: 'Review the dashboard overview and current system information.' },
    { number: 3, text: 'Open Employees, Departments, Attendance, Leave, or Payroll as needed.' },
    { number: 4, text: 'Use the ⓘ Need help? beside a function whenever you need guidance.' }
  ];
}
