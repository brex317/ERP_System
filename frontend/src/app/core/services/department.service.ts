import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { DepartmentDto } from '../models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private apiUrl = 'http://localhost:5000/api/departments';

  constructor(private http: HttpClient) {}

  getDepartments(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching departments from backend:', error);
        return of([
          { id: 1, name: 'Information Technology', code: 'IT', description: 'Software Development & IT Infrastructure' },
          { id: 2, name: 'Human Resources', code: 'HR', description: 'Talent Acquisition & Employee Welfare' },
          { id: 3, name: 'Finance & Accounting', code: 'FIN', description: 'Financial Planning & Accounting' },
          { id: 4, name: 'Operations', code: 'OPS', description: 'Business Operations & Supply Chain' }
        ]);
      })
    );
  }

  getDepartment(id: number): Observable<DepartmentDto> {
    return this.http.get<DepartmentDto>(`${this.apiUrl}/${id}`);
  }

  createDepartment(department: Partial<DepartmentDto>): Observable<DepartmentDto> {
    return this.http.post<DepartmentDto>(this.apiUrl, department);
  }

  updateDepartment(id: number, department: Partial<DepartmentDto>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, department);
  }

  deleteDepartment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
