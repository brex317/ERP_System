export interface PayrollRecordDto {
  id: number;
  employeeId: number;
  employeeName: string;
  departmentName: string;
  payPeriod: string;
  baseSalary: number;
  allowances: number;
  deductions: number;
  netSalary: number;
  status: string;
  processedDate: string;
}

export interface ProcessPayrollDto {
  employeeId: number;
  payPeriod: string;
  baseSalary: number;
  allowances: number;
  deductions: number;
}
