export interface OfficerGrievance {
  id: number;
  grievanceNumber: string;
  citizenName: string;
  category: string;
  description: string;
  status: string;
  createdAt: string;
  isEscalated: boolean;
  assignedOfficerId: number;
  resolutionRemarks?: string;
}


export enum GrievanceStatus {
  Submitted = 1,
  Assigned = 2,
  InReview = 3,
  Resolved = 4,
  Closed = 5
}

export interface UpdateGrievanceStatusRequest {
  status: GrievanceStatus;
  resolutionRemarks?: string;
}
