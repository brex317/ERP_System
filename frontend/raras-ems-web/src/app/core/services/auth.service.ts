import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap, catchError, of } from 'rxjs';
import { LoginResponse, UserProfile } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth';
  private currentUserSubject = new BehaviorSubject<UserProfile | null>(this.getStoredUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  login(identifier: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, {
      email: identifier,
      username: identifier,
      password
    }).pipe(
      tap(response => {
        if (response && response.success && response.token) {
          localStorage.setItem('raras_token', response.token);
          localStorage.setItem('raras_user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        }
      })
    );
  }

  logout(): void {
    // Fire-and-forget optional logout notify to backend
    this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      catchError(() => of(null))
    ).subscribe();

    this.clearSessionAndRedirect();
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('raras_token');
    return !!token && !!this.currentUserSubject.value;
  }

  getCurrentUser(): UserProfile | null {
    return this.currentUserSubject.value;
  }

  private clearSessionAndRedirect(): void {
    localStorage.removeItem('raras_token');
    localStorage.removeItem('raras_user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  private getStoredUser(): UserProfile | null {
    const token = localStorage.getItem('raras_token');
    const userStr = localStorage.getItem('raras_user');
    if (!token || !userStr) {
      return null;
    }
    try {
      return JSON.parse(userStr);
    } catch {
      return null;
    }
  }
}
