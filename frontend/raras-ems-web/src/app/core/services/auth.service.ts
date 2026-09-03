import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginResponse, UserProfile } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth';
  private currentUserSubject = new BehaviorSubject<UserProfile | null>(this.getStoredUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        if (response.success) {
          localStorage.setItem('raras_token', response.token);
          localStorage.setItem('raras_user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('raras_token');
    localStorage.removeItem('raras_user');
    this.currentUserSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('raras_token');
  }

  private getStoredUser(): UserProfile | null {
    const userStr = localStorage.getItem('raras_user');
    if (!userStr) return { name: 'Berihu', initials: 'BE', email: 'berihu@raras.com', role: 'Administrator' };
    try {
      return JSON.parse(userStr);
    } catch {
      return null;
    }
  }
}
