export interface AttendanceDto {
  id: number;
  employeeId: number;
  employeeName?: string;
  date: string;
  checkIn?: string;
  checkOut?: string;
  status: string;
  createdAt?: string;
}
