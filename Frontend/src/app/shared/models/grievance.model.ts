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
  description: string;
  resolvedAt: string;
  resolutionRemarks: string;
  isEscalated: boolean;
  escalatedAt: string;
  feedback?: string;
  rating?: number;
  reopenedAt?: string;
}

