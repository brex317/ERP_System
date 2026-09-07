import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, map } from 'rxjs';
import { GlobalSearchResponseDto, SearchResultItemDto } from '../models/search.model';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = 'http://localhost:5000/api/search';

  private mockSystemItems: SearchResultItemDto[] = [
    { title: 'John Doe', subtitle: 'Software Developer • john.doe@raras.com', category: 'Employees', linkUrl: '/employees' },
    { title: 'Sara Smith', subtitle: 'Accountant • sara.smith@raras.com', category: 'Employees', linkUrl: '/employees' },
    { title: 'Abebe Bikila', subtitle: 'Operations Manager • abebe.b@raras.com', category: 'Employees', linkUrl: '/employees' },
    { title: 'Information Technology', subtitle: 'Code: IT • Software & IT Support', category: 'Departments', linkUrl: '/departments' },
    { title: 'Human Resources', subtitle: 'Code: HR • Recruitment & Welfare', category: 'Departments', linkUrl: '/departments' },
    { title: 'Finance & Accounting', subtitle: 'Code: FIN • Financial Planning', category: 'Departments', linkUrl: '/departments' },
    { title: 'Attendance Tracking', subtitle: 'Check-in/out, logs & shift status', category: 'System Modules', linkUrl: '/attendance' },
    { title: 'Leave Management', subtitle: 'Request time off & track leave balances', category: 'System Modules', linkUrl: '/leave' },
    { title: 'Payroll & Salaries', subtitle: 'Process monthly salaries & view payslips', category: 'System Modules', linkUrl: '/payroll' },
    { title: 'Help & Knowledge Base', subtitle: 'System user guides & step-by-step documentation', category: 'System Modules', linkUrl: '/help' },
    { title: 'Contact Support Desk', subtitle: 'Submit tickets & contact system admin', category: 'System Modules', linkUrl: '/support' },
    { title: 'System & Profile Settings', subtitle: 'Configure profile, security & EMS preferences', category: 'System Modules', linkUrl: '/settings' }
  ];

  constructor(private http: HttpClient) {}

  search(query: string): Observable<GlobalSearchResponseDto> {
    const q = (query || '').trim();
    if (!q) {
      return of({ query: '', results: [] });
    }

    return this.http.get<GlobalSearchResponseDto>(`${this.apiUrl}?q=${encodeURIComponent(q)}`).pipe(
      map(res => {
        if (res && res.results && res.results.length > 0) {
          return res;
        }
        return { query: q, results: this.localSearch(q) };
      }),
      catchError(() => {
        return of({ query: q, results: this.localSearch(q) });
      })
    );
  }

  private localSearch(q: string): SearchResultItemDto[] {
    const lower = q.toLowerCase();
    return this.mockSystemItems.filter(item =>
      item.title.toLowerCase().includes(lower) ||
      item.subtitle.toLowerCase().includes(lower) ||
      item.category.toLowerCase().includes(lower)
    );
  }
}
