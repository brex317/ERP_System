import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { SupportTicketDto, CreateSupportTicketDto } from '../models/support.model';

@Injectable({
  providedIn: 'root'
})
export class SupportService {
  private apiUrl = 'http://localhost:5000/api/support';

  constructor(private http: HttpClient) {}

  getTickets(): Observable<SupportTicketDto[]> {
    return this.http.get<SupportTicketDto[]>(this.apiUrl).pipe(
      catchError(() => of([
        { id: 1, subject: 'Payroll calculation discrepancy', category: 'Payroll', priority: 'High', message: 'Deductions calculated differently for August.', status: 'Open', submittedBy: 'admin', createdAt: new Date().toISOString() },
        { id: 2, subject: 'Unable to request leave', category: 'Leave', priority: 'Medium', message: 'Leave balance displays 0 available days.', status: 'In Progress', submittedBy: 'john.doe', createdAt: new Date().toISOString() }
      ]))
    );
  }

  createTicket(dto: CreateSupportTicketDto): Observable<SupportTicketDto> {
    return this.http.post<SupportTicketDto>(this.apiUrl, dto);
  }

  updateTicketStatus(id: number, status: string, adminNotes?: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/status`, { status, adminNotes });
  }
}
