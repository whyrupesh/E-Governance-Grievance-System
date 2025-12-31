export interface OfficerGrievance {
  id: number;
  grievanceNumber: string;
  citizenName: string;
  category: string;
  description: string;
  status: string;
  createdAt: string;
}

export interface UpdateGrievanceStatusRequest {
  status: string;
  remarks: string;
}
