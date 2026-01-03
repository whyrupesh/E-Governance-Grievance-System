export interface SupervisorGrievance {
  id: number;
  grievanceNumber: string;
  category: string;
  department: string;
  status: string;
  createdAt: string;
  description : string;
  resolveAt : string;
  resolutionRemarks : string;
  isEscalated: boolean;
  escalatedAt : string;
}
