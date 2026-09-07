import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { UserProfile } from '../../core/models/user.model';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  activeTab: string = 'profile';
  currentUser: UserProfile | null = null;
  savedMessage: string = '';

  profileData = {
    name: '',
    email: '',
    phone: '+251 91 123 4567',
    department: 'Administration',
    jobTitle: 'System Administrator'
  };

  preferences = {
    theme: 'light',
    language: 'English',
    timezone: 'East Africa Time (UTC+3)',
    dateFormat: 'YYYY-MM-DD'
  };

  security = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
    enable2FA: false
  };

  notifications = {
    emailAlerts: true,
    leaveRequestsNotify: true,
    payrollUpdatesNotify: true,
    systemAnnouncements: true
  };

  systemConfig = {
    companyName: 'RARAS Tech Solutions',
    workStart: '08:30',
    workEnd: '17:30',
    currency: 'USD ($)',
    defaultLeaveDays: 20
  };

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user) {
        this.profileData.name = user.name;
        this.profileData.email = user.email;
      }
    });
  }

  setTab(tab: string): void {
    this.activeTab = tab;
  }

  saveSettings(): void {
    this.savedMessage = 'Settings updated successfully!';
    setTimeout(() => this.savedMessage = '', 4000);
  }
}
