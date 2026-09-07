import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DepartmentService } from '../../core/services/department.service';
import { DepartmentDto } from '../../core/models/department.model';
import { FunctionalityHelpComponent } from '../../shared/components/functionality-help/functionality-help.component';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FormsModule, FunctionalityHelpComponent],
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  departments: DepartmentDto[] = [];
  isLoading: boolean = true;
  searchQuery: string = '';

  // Modal state
  isModalOpen: boolean = false;
  isEditMode: boolean = false;
  editingDeptId: number | null = null;
  isSubmitting: boolean = false;
  errorMessage: string = '';

  formData: Partial<DepartmentDto> = {
    name: '',
    code: '',
    description: ''
  };

  constructor(private departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.isLoading = true;
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load departments:', err);
        this.isLoading = false;
      }
    });
  }

  get filteredDepartments(): DepartmentDto[] {
    if (!this.searchQuery.trim()) return this.departments;
    const q = this.searchQuery.toLowerCase();
    return this.departments.filter(d =>
      d.name.toLowerCase().includes(q) ||
      (d.code && d.code.toLowerCase().includes(q)) ||
      (d.description && d.description.toLowerCase().includes(q))
    );
  }

  openAddModal(): void {
    this.isEditMode = false;
    this.editingDeptId = null;
    this.errorMessage = '';
    this.formData = { name: '', code: '', description: '' };
    this.isModalOpen = true;
  }

  openEditModal(dept: DepartmentDto): void {
    this.isEditMode = true;
    this.editingDeptId = dept.id;
    this.errorMessage = '';
    this.formData = {
      name: dept.name,
      code: dept.code || '',
      description: dept.description || ''
    };
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  saveDepartment(): void {
    if (!this.formData.name?.trim() || !this.formData.code?.trim()) {
      this.errorMessage = 'Department name and code are required.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    if (this.isEditMode && this.editingDeptId) {
      this.departmentService.updateDepartment(this.editingDeptId, this.formData).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.closeModal();
          this.loadDepartments();
        },
        error: () => {
          this.isSubmitting = false;
          this.errorMessage = 'Failed to update department.';
        }
      });
    } else {
      this.departmentService.createDepartment(this.formData).subscribe({
        next: (created) => {
          this.isSubmitting = false;
          this.closeModal();
          this.departments.push(created);
        },
        error: () => {
          this.isSubmitting = false;
          this.errorMessage = 'Failed to create department.';
        }
      });
    }
  }

  deleteDepartment(id: number, name: string): void {
    if (confirm(`Are you sure you want to delete department "${name}"?`)) {
      this.departmentService.deleteDepartment(id).subscribe({
        next: () => {
          this.departments = this.departments.filter(d => d.id !== id);
        },
        error: (err) => console.error('Failed to delete department', err)
      });
    }
  }
}
