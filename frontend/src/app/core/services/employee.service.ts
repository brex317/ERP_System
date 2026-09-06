import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { EmployeeDto, CreateEmployeeDto } from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5000/api/employees';

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching employees from backend:', error);
        return of([
          { id: 1, firstName: 'John', lastName: 'Doe', email: 'john.doe@raras.com', departmentName: 'IT', position: 'Software Developer', status: 'Active' },
          { id: 2, firstName: 'Sara', lastName: 'Smith', email: 'sara.smith@raras.com', departmentName: 'Finance', position: 'Accountant', status: 'Active' },
          { id: 3, firstName: 'Abebe', lastName: 'Bikila', email: 'abebe.b@raras.com', departmentName: 'Operations', position: 'Operations Manager', status: 'Active' },
          { id: 4, firstName: 'Tigist', lastName: 'Haile', email: 'tigist.h@raras.com', departmentName: 'HR', position: 'HR Specialist', status: 'Active' },
          { id: 5, firstName: 'Berihu', lastName: 'Tadesse', email: 'berihu.t@raras.com', departmentName: 'IT', position: 'System Admin', status: 'Active' }
        ]);
      })
    );
  }

  getEmployee(id: number): Observable<EmployeeDto> {
    return this.http.get<EmployeeDto>(`${this.apiUrl}/${id}`);
  }

  createEmployee(dto: CreateEmployeeDto): Observable<EmployeeDto> {
    return this.http.post<EmployeeDto>(this.apiUrl, dto);
  }

  updateEmployee(id: number, dto: Partial<CreateEmployeeDto>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
