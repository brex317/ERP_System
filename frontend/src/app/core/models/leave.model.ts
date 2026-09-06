export interface LeaveRequestDto {
  id: number;
  employeeId: number;
  employeeName?: string;
  leaveType: string;
  startDate: string;
  endDate: string;
  reason?: string;
  status: string;
  createdAt?: string;
}
