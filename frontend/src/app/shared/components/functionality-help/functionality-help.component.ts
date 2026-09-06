import { Component, Input, OnInit, OnChanges, SimpleChanges, OnDestroy, ElementRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { HelpService, HelpStep } from '../../../core/services/help.service';
import { HelpContextResolverService } from '../../../core/services/help-context-resolver.service';

@Component({
  selector: 'app-functionality-help, app-need-help',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="functionality-help" [class.align-left]="align === 'left'" [class.active]="isOpen">
      <button 
        type="button"
        class="functionality-help-trigger" 
        (click)="toggleMenu($event)"
        aria-label="Need help"
        [attr.aria-expanded]="isOpen"
      >
        ⓘ Need help?
      </button>

      <div class="functionality-help-menu" (click)="$event.stopPropagation()">
        <div class="functionality-help-header">
          <div class="functionality-help-title">{{ title }}</div>
          <button type="button" class="close-btn" (click)="closeMenu($event)" aria-label="Close help">&times;</button>
        </div>

        <div *ngIf="isLoading" class="functionality-help-loading">
          <span class="btn-spinner"></span> Loading help steps...
        </div>

        <ng-container *ngIf="!isLoading">
          <div *ngFor="let step of steps" class="functionality-help-step">
            <span class="functionality-help-number">{{ step.number }}</span>
            <span>{{ step.text }}</span>
          </div>

          <div *ngIf="steps.length === 0" class="functionality-help-empty">
            No help instructions found for this page context.
          </div>
        </ng-container>
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
    .functionality-help.align-left {
        margin-left: 0;
        margin-right: auto;
    }
    .functionality-help-trigger {
        display: inline-flex;
        align-items: center;
        border: 0;
        background: transparent;
        color: #2563eb;
        padding: 6px 10px;
        margin: 0;
        font-size: 13px;
        font-weight: 600;
        line-height: 1.4;
        white-space: nowrap;
        cursor: pointer;
        text-decoration: none;
        border-radius: 6px;
        transition: background-color 0.2s ease, color 0.2s ease;
    }
    .functionality-help-trigger:hover,
    .functionality-help.active .functionality-help-trigger {
        background-color: #eff6ff;
        color: #1d4ed8;
    }
    .functionality-help-menu {
        position: absolute;
        top: calc(100% + 8px);
        right: 0;
        width: min(340px, calc(100vw - 32px));
        max-width: 340px;
        background: #ffffff;
        border: 1px solid #dbe3ef;
        border-radius: 12px;
        box-shadow: 0 16px 36px rgba(15, 23, 42, 0.16);
        padding: 16px;
        visibility: hidden;
        opacity: 0;
        transform: translateY(-6px);
        pointer-events: none;
        transition: opacity .18s ease, transform .18s ease, visibility .18s ease;
        z-index: 10000;
    }
    .functionality-help.align-left .functionality-help-menu {
        right: auto;
        left: 0;
    }
    .functionality-help:hover .functionality-help-menu,
    .functionality-help.active .functionality-help-menu {
        visibility: visible;
        opacity: 1;
        transform: translateY(0);
        pointer-events: auto;
    }
    .functionality-help-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 10px;
        padding-bottom: 8px;
        border-bottom: 1px solid #f1f5f9;
    }
    .functionality-help-title {
        color: #1e293b;
        font-size: 14px;
        font-weight: 700;
    }
    .close-btn {
        background: transparent;
        border: none;
        font-size: 18px;
        color: #94a3b8;
        cursor: pointer;
        padding: 0 4px;
        line-height: 1;
    }
    .close-btn:hover {
        color: #475569;
    }
    .functionality-help-loading {
        font-size: 12px;
        color: #64748b;
        padding: 12px 0;
        display: flex;
        align-items: center;
        gap: 8px;
    }
    .btn-spinner {
        width: 14px;
        height: 14px;
        border: 2px solid #cbd5e1;
        border-top-color: #2563eb;
        border-radius: 50%;
        animation: spin 0.8s linear infinite;
    }
    @keyframes spin {
        to { transform: rotate(360deg); }
    }
    .functionality-help-step {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        padding: 9px 0;
        border-bottom: 1px solid #f8fafc;
        color: #475569;
        font-size: 12px;
        line-height: 1.5;
    }
    .functionality-help-step:last-child {
        border-bottom: 0;
        padding-bottom: 0;
    }
    .functionality-help-number {
        width: 22px;
        height: 22px;
        flex: 0 0 22px;
        border-radius: 50%;
        background: #eff6ff;
        color: #2563eb;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 11px;
        font-weight: 700;
    }
    .functionality-help-empty {
        font-size: 12px;
        color: #94a3b8;
        padding: 12px 0;
        text-align: center;
    }
  `]
})
export class FunctionalityHelpComponent implements OnInit, OnChanges, OnDestroy {
  @Input() moduleKey?: string;
  @Input() pageKey?: string;
  @Input() functionalityKey?: string;
  @Input() title: string = 'Quick steps';
  @Input() steps: HelpStep[] = [];
  @Input() align: 'left' | 'right' = 'right';

  public isOpen: boolean = false;
  public isLoading: boolean = false;
  private routeSub?: Subscription;

  constructor(
    private readonly helpService: HelpService,
    private readonly helpResolver: HelpContextResolverService,
    private readonly elementRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.loadHelpData();
    this.routeSub = this.helpResolver.currentKeys$.subscribe(() => {
      this.loadHelpData();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['moduleKey'] || changes['pageKey'] || changes['functionalityKey']) {
      this.loadHelpData();
    }
  }

  ngOnDestroy(): void {
    this.routeSub?.unsubscribe();
  }

  public toggleMenu(event: MouseEvent): void {
    event.stopPropagation();
    this.isOpen = !this.isOpen;
    if (this.isOpen) {
      this.loadHelpData();
    }
  }

  public closeMenu(event?: MouseEvent): void {
    if (event) {
      event.stopPropagation();
    }
    this.isOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }

  private loadHelpData(): void {
    const resolved = this.helpResolver.getCurrentKeys();
    const finalModule = (this.moduleKey || resolved.module || '').trim();
    const finalPage = (this.pageKey || resolved.page || '').trim();
    const finalFunctionality = (this.functionalityKey || resolved.functionality || '').trim();

    this.isLoading = true;
    this.helpService.getHelp(finalModule, finalPage, finalFunctionality).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res) {
          if (res.title) {
            this.title = res.title;
          }
          if (res.steps && res.steps.length > 0) {
            this.steps = res.steps;
          } else {
            // Fallback steps if database returns empty list for an unseeded custom page
            this.steps = this.getFallbackSteps(finalModule, finalPage);
          }
        }
      },
      error: (err) => {
        console.error('Failed to load help steps from backend:', err);
        this.isLoading = false;
        this.steps = this.getFallbackSteps(finalModule, finalPage);
      }
    });
  }

  private getFallbackSteps(moduleKey: string, pageKey: string): HelpStep[] {
    const page = pageKey.toLowerCase();
    const mod = moduleKey.toLowerCase();

    if (page === 'employee-list' || mod === 'employees') {
      return [
        { number: 1, text: 'Open Employees from the sidebar.' },
        { number: 2, text: 'Click Add Employee to open the employee registration form.' },
        { number: 3, text: 'Enter the required personal and employment information.' },
        { number: 4, text: 'Select the employee\'s department and position.' },
        { number: 5, text: 'Click Save Employee to complete registration.' }
      ];
    }
    if (page === 'department-list' || mod === 'departments') {
      return [
        { number: 1, text: 'Open Departments from the sidebar.' },
        { number: 2, text: 'Click Add Department.' },
        { number: 3, text: 'Enter the department name and required information.' },
        { number: 4, text: 'Review the department details.' },
        { number: 5, text: 'Save the department.' }
      ];
    }
    if (page === 'attendance-list' || mod === 'attendance') {
      return [
        { number: 1, text: 'Open Attendance from the sidebar.' },
        { number: 2, text: 'Select the employee whose attendance you want to record.' },
        { number: 3, text: 'Select the correct attendance status.' },
        { number: 4, text: 'Check the attendance date and details.' },
        { number: 5, text: 'Save the attendance record.' }
      ];
    }
    if (page === 'leave-list' || mod === 'leave') {
      return [
        { number: 1, text: 'Open Leave Management from the sidebar.' },
        { number: 2, text: 'Click New Request.' },
        { number: 3, text: 'Select the employee and leave type.' },
        { number: 4, text: 'Select the start and end dates.' },
        { number: 5, text: 'Submit the leave request.' }
      ];
    }
    if (page === 'payroll-list' || mod === 'payroll') {
      return [
        { number: 1, text: 'Open Payroll from the sidebar.' },
        { number: 2, text: 'Review the employee salary information.' },
        { number: 3, text: 'Verify the payroll details before processing.' },
        { number: 4, text: 'Check the calculated payroll information.' },
        { number: 5, text: 'Process payroll according to your organization workflow.' }
      ];
    }
    return [
      { number: 1, text: 'Use the sidebar to open the module you need.' },
      { number: 2, text: 'Review the page overview and current system information.' },
      { number: 3, text: 'Perform your required operations.' },
      { number: 4, text: 'Use the ⓘ Need help? button whenever you need guidance.' }
    ];
  }
}

export { HelpStep } from '../../../core/services/help.service';
