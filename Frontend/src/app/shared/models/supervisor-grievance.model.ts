export interface SupervisorGrievance {
  grievanceNumber: string;
  department: string;
  category: string;
  status: string;
  createdAt: string;
  daysPending: number;
  isEscalated: boolean;
}
