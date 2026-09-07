import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { LeaveRequestDto } from '../models/leave.model';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  private apiUrl = 'http://localhost:5000/api/leave';

  constructor(private http: HttpClient) {}

  getLeaveRequests(): Observable<LeaveRequestDto[]> {
    return this.http.get<LeaveRequestDto[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching leave requests from backend:', error);
        return of([
          { id: 1, employeeId: 4, employeeName: 'Tigist Haile', leaveType: 'Annual Leave', startDate: '2026-09-01', endDate: '2026-09-07', reason: 'Vacation', status: 'Approved' },
          { id: 2, employeeId: 1, employeeName: 'John Doe', leaveType: 'Sick Leave', startDate: '2026-09-10', endDate: '2026-09-11', reason: 'Medical appointment', status: 'Pending' }
        ]);
      })
    );
  }

  createLeaveRequest(request: Partial<LeaveRequestDto>): Observable<LeaveRequestDto> {
    return this.http.post<LeaveRequestDto>(this.apiUrl, request);
  }

  updateLeaveStatus(id: number, status: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/status`, { status });
  }
}
