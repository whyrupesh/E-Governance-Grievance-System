export interface CreateGrievanceRequest {
  categoryId: number;
  description: string;
}

export interface Grievance {
  id: number;
  grievanceNumber: string;
  category: string;
  department: string;
  status: string;
  createdAt: string;
}
