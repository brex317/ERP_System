import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, catchError, of, tap } from 'rxjs';
import { NotificationDto } from '../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'http://localhost:5000/api/notifications';
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private http: HttpClient) {}

  getNotifications(): Observable<NotificationDto[]> {
    return this.http.get<NotificationDto[]>(this.apiUrl).pipe(
      tap(list => {
        const count = list.filter(n => !n.isRead).length;
        this.unreadCountSubject.next(count);
      }),
      catchError(() => {
        const mock: NotificationDto[] = [
          { id: 1, title: 'Leave Request Submitted', message: 'John Doe submitted a new leave request.', type: 'Leave', isRead: false, createdAt: new Date().toISOString(), link: '/leave' },
          { id: 2, title: 'Payroll Processing', message: 'September payroll records generated.', type: 'Payroll', isRead: false, createdAt: new Date().toISOString(), link: '/payroll' },
          { id: 3, title: 'System Update', message: 'RARAS EMS version 2.0 deployed successfully.', type: 'System', isRead: true, createdAt: new Date().toISOString() }
        ];
        this.unreadCountSubject.next(mock.filter(n => !n.isRead).length);
        return of(mock);
      })
    );
  }

  markAsRead(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/read`, {}).pipe(
      tap(() => {
        const current = this.unreadCountSubject.value;
        if (current > 0) this.unreadCountSubject.next(current - 1);
      }),
      catchError(() => of(void 0))
    );
  }

  markAllAsRead(): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/read-all`, {}).pipe(
      tap(() => this.unreadCountSubject.next(0)),
      catchError(() => of(void 0))
    );
  }
}
