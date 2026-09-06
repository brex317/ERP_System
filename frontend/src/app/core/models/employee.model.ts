export interface EmployeeDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  departmentId?: number;
  departmentName?: string;
  position?: string;
  status: string;
  hireDate?: string;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  departmentId?: number;
  position?: string;
  status?: string;
  hireDate?: string;
}
