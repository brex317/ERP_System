export interface SupportTicketDto {
  id: number;
  subject: string;
  category: string;
  priority: string;
  message: string;
  status: string;
  submittedBy: string;
  createdAt: string;
  adminNotes?: string;
}

export interface CreateSupportTicketDto {
  subject: string;
  category: string;
  priority: string;
  message: string;
}
