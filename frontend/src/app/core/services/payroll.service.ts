import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { PayrollRecordDto, ProcessPayrollDto } from '../models/payroll.model';

@Injectable({
  providedIn: 'root'
})
export class PayrollService {
  private apiUrl = 'http://localhost:5000/api/payroll';

  constructor(private http: HttpClient) {}

  getPayrollRecords(payPeriod?: string): Observable<PayrollRecordDto[]> {
    const url = payPeriod ? `${this.apiUrl}?payPeriod=${payPeriod}` : this.apiUrl;
    return this.http.get<PayrollRecordDto[]>(url).pipe(
      catchError(() => of([
        { id: 1, employeeId: 1, employeeName: 'John Doe', departmentName: 'IT', payPeriod: '2026-09', baseSalary: 4500, allowances: 500, deductions: 450, netSalary: 4550, status: 'Paid', processedDate: '2026-09-01' },
        { id: 2, employeeId: 2, employeeName: 'Sara Smith', departmentName: 'Finance', payPeriod: '2026-09', baseSalary: 3800, allowances: 300, deductions: 380, netSalary: 3720, status: 'Paid', processedDate: '2026-09-01' }
      ]))
    );
  }

  processPayroll(dto: ProcessPayrollDto): Observable<PayrollRecordDto> {
    return this.http.post<PayrollRecordDto>(this.apiUrl, dto);
  }
}
