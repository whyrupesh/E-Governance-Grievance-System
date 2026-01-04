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
  assignedTo : string;
  feedback : string;
  departmentId : number;
  closingRemarks : string;
}
