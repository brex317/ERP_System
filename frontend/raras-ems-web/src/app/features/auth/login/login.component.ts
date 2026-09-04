import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { FunctionalityHelpComponent } from '../../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  template: `
    <div class="login-container">
      <div class="login-card">
        <div class="login-top-bar">
          <app-functionality-help 
            moduleKey="auth" 
            pageKey="login" 
            functionalityKey="login-form"
            align="left">
          </app-functionality-help>
        </div>

        <div class="login-header">
          <div class="logo">
            RARAS <span>EMS</span>
          </div>
          <h2>Welcome Back</h2>
          <p>Sign in to your Employee Management account</p>
        </div>

        <form (ngSubmit)="onLogin()" class="login-form">
          <div *ngIf="errorMessage" class="error-banner">
            {{ errorMessage }}
          </div>

          <div class="form-group">
            <label for="email">Username or Email Address</label>
            <input 
              type="text" 
              id="email" 
              name="email" 
              [(ngModel)]="email" 
              placeholder="admin@raras.com or admin" 
              required
              [disabled]="isLoading"
            >
          </div>

          <div class="form-group">
            <label for="password">Password</label>
            <input 
              type="password" 
              id="password" 
              name="password" 
              [(ngModel)]="password" 
              placeholder="••••••••" 
              required
              [disabled]="isLoading"
            >
          </div>

          <div class="form-options">
            <label class="remember-me">
              <input type="checkbox" checked> Remember me
            </label>
            <a href="#" class="forgot-password" (click)="$event.preventDefault()">Forgot password?</a>
          </div>

          <button type="submit" class="submit-btn" [disabled]="isLoading">
            <span *ngIf="!isLoading">Sign In →</span>
            <span *ngIf="isLoading" class="spinner-text">
              <span class="btn-spinner"></span> Authenticating...
            </span>
          </button>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background: linear-gradient(135deg, #f8fafc 0%, #eff6ff 100%);
        padding: 24px;
    }
    .login-card {
        width: 100%;
        max-width: 420px;
        background: #ffffff;
        border: 1px solid #e2e8f0;
        border-radius: 14px;
        padding: 28px 32px 36px;
        box-shadow: 0 10px 25px rgba(15, 23, 42, 0.06);
        position: relative;
    }
    .login-top-bar {
        display: flex;
        justify-content: flex-start;
        align-items: center;
        margin-bottom: 12px;
    }
    .login-header {
        text-align: center;
        margin-bottom: 28px;
    }
    .logo {
        font-size: 24px;
        font-weight: 800;
        color: #2563eb;
        margin-bottom: 12px;
    }
    .logo span {
        color: #64748b;
        font-size: 12px;
        margin-left: 6px;
        font-weight: 600;
    }
    .login-header h2 {
        font-size: 22px;
        color: #1e293b;
        margin-bottom: 6px;
    }
    .login-header p {
        font-size: 13px;
        color: #64748b;
    }
    .error-banner {
        background: #fef2f2;
        border: 1px solid #fecaca;
        color: #dc2626;
        padding: 10px 14px;
        border-radius: 8px;
        font-size: 12px;
        margin-bottom: 18px;
        display: flex;
        align-items: center;
        gap: 8px;
    }
    .form-group {
        margin-bottom: 18px;
    }
    .form-group label {
        display: block;
        font-size: 13px;
        font-weight: 600;
        color: #475569;
        margin-bottom: 6px;
    }
    .form-group input {
        width: 100%;
        padding: 11px 14px;
        border: 1px solid #e2e8f0;
        border-radius: 8px;
        font-size: 13px;
        background: #f8fafc;
        outline: none;
        transition: all 0.2s ease;
    }
    .form-group input:focus {
        border-color: #2563eb;
        background: #ffffff;
        box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
    }
    .form-options {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 12px;
        margin-bottom: 22px;
    }
    .remember-me {
        color: #64748b;
        display: flex;
        align-items: center;
        gap: 6px;
        cursor: pointer;
    }
    .forgot-password {
        color: #2563eb;
        font-weight: 600;
        text-decoration: none;
    }
    .submit-btn {
        width: 100%;
        padding: 12px;
        background: #2563eb;
        color: #ffffff;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s ease;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .submit-btn:hover:not(:disabled) {
        background: #1d4ed8;
        transform: translateY(-1px);
    }
    .submit-btn:disabled {
        opacity: 0.7;
        cursor: not-allowed;
    }
    .spinner-text {
        display: inline-flex;
        align-items: center;
        gap: 8px;
    }
    .btn-spinner {
        width: 14px;
        height: 14px;
        border: 2px solid rgba(255, 255, 255, 0.3);
        border-radius: 50%;
        border-top-color: #ffffff;
        animation: spin 0.8s linear infinite;
    }
    @keyframes spin {
        to { transform: rotate(360deg); }
    }
  `]
})
export class LoginComponent {
  email: string = 'admin@raras.com';
  password: string = 'admin123';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter your username/email and password.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response && response.success) {
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = response?.message || 'Invalid credentials.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401 && err.error?.message) {
          this.errorMessage = err.error.message;
        } else if (err.error?.message) {
          this.errorMessage = err.error.message;
        } else {
          this.errorMessage = 'Unable to connect to authentication server. Please check your backend connection.';
        }
      }
    });
  }
}
