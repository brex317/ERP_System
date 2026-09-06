import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { DashboardStats } from '../models/dashboard-stats.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'http://localhost:5000/api/dashboard';

  constructor(private http: HttpClient) {}

  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/stats`).pipe(
      catchError(error => {
        console.error('Error fetching dashboard stats from API:', error);
        // Default fallback if backend is unreachable
        return of({
          totalEmployees: 248,
          totalDepartments: 12,
          presentToday: 221,
          onLeave: 18
        });
      })
    );
  }
}
