import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { AttendanceDto } from '../models/attendance.model';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5000/api/attendance';

  constructor(private http: HttpClient) {}

  getAttendance(date?: string): Observable<AttendanceDto[]> {
    const url = date ? `${this.apiUrl}?date=${date}` : this.apiUrl;
    return this.http.get<AttendanceDto[]>(url).pipe(
      catchError(error => {
        console.error('Error fetching attendance from backend:', error);
        return of([
          { id: 1, employeeId: 1, employeeName: 'John Doe', date: new Date().toISOString(), checkIn: '08:30 AM', checkOut: '05:00 PM', status: 'Present' },
          { id: 2, employeeId: 2, employeeName: 'Sara Smith', date: new Date().toISOString(), checkIn: '08:45 AM', checkOut: '05:15 PM', status: 'Present' },
          { id: 3, employeeId: 3, employeeName: 'Abebe Bikila', date: new Date().toISOString(), checkIn: '09:00 AM', checkOut: '05:30 PM', status: 'Late' },
          { id: 4, employeeId: 4, employeeName: 'Tigist Haile', date: new Date().toISOString(), checkIn: '-', checkOut: '-', status: 'On Leave' }
        ]);
      })
    );
  }

  logAttendance(record: Partial<AttendanceDto>): Observable<AttendanceDto> {
    return this.http.post<AttendanceDto>(this.apiUrl, record);
  }
}
