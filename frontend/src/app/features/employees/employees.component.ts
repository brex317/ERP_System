import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../core/services/employee.service';
import { DepartmentService } from '../../core/services/department.service';
import { EmployeeDto, CreateEmployeeDto } from '../../core/models/employee.model';
import { DepartmentDto } from '../../core/models/department.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {
  employees: EmployeeDto[] = [];
  departments: DepartmentDto[] = [];
  isLoading: boolean = true;
  searchQuery: string = '';

  // Modal State
  isModalOpen: boolean = false;
  isEditMode: boolean = false;
  editingEmployeeId: number | null = null;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  formData: CreateEmployeeDto = {
    firstName: '',
    lastName: '',
    email: '',
    departmentId: undefined,
    position: '',
    status: 'Active'
  };

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartments();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load employees:', err);
        this.isLoading = false;
      }
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (err) => console.error('Failed to load departments', err)
    });
  }

  get filteredEmployees(): EmployeeDto[] {
    if (!this.searchQuery.trim()) return this.employees;
    const q = this.searchQuery.toLowerCase();
    return this.employees.filter(e =>
      e.firstName.toLowerCase().includes(q) ||
      e.lastName.toLowerCase().includes(q) ||
      e.email.toLowerCase().includes(q) ||
      (e.departmentName && e.departmentName.toLowerCase().includes(q)) ||
      (e.position && e.position.toLowerCase().includes(q))
    );
  }

  openAddModal(): void {
    this.isEditMode = false;
    this.editingEmployeeId = null;
    this.errorMessage = '';
    this.formData = {
      firstName: '',
      lastName: '',
      email: '',
      departmentId: this.departments.length > 0 ? this.departments[0].id : undefined,
      position: '',
      status: 'Active'
    };
    this.isModalOpen = true;
  }

  openEditModal(emp: EmployeeDto): void {
    this.isEditMode = true;
    this.editingEmployeeId = emp.id;
    this.errorMessage = '';
    this.formData = {
      firstName: emp.firstName,
      lastName: emp.lastName,
      email: emp.email,
      departmentId: emp.departmentId,
      position: emp.position || '',
      status: emp.status || 'Active'
    };
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  saveEmployee(): void {
    if (!this.formData.firstName.trim() || !this.formData.lastName.trim() || !this.formData.email.trim()) {
      this.errorMessage = 'First name, last name, and email are required.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    if (this.isEditMode && this.editingEmployeeId) {
      this.employeeService.updateEmployee(this.editingEmployeeId, this.formData).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.closeModal();
          this.loadEmployees();
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Failed to update employee.';
        }
      });
    } else {
      this.employeeService.createEmployee(this.formData).subscribe({
        next: (created) => {
          this.isSubmitting = false;
          this.closeModal();
          this.employees.unshift(created);
        },
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Failed to create employee.';
        }
      });
    }
  }

  deleteEmployee(id: number, name: string): void {
    if (confirm(`Are you sure you want to delete employee ${name}?`)) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.employees = this.employees.filter(e => e.id !== id);
        },
        error: (err) => console.error('Failed to delete employee', err)
      });
    }
  }
}
