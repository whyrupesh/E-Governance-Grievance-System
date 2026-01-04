import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Injectable({
  providedIn: 'root'
})
export class SupervisorService {

  private readonly API_URL =
    'http://localhost:5280/api/supervisor';

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<SupervisorGrievance[]>(`${this.API_URL}/grievances`);
  }

  getById(grievanceId: number) {
    return this.http.get<SupervisorGrievance>(
      `${this.API_URL}/grievances/${grievanceId}`
    );
  }

  getOverdue(days: number) {
    return this.http.get<SupervisorGrievance[]>(
      `${this.API_URL}/overdue?days=${days}`
    );
  }

  escalateGrievance(grievanceId: number) {
    return this.http.patch(
      `${this.API_URL}/escalate/${grievanceId}`,
      {}
    );
  }

  assignOfficer(grievanceId: number, staffId: number) {
    return this.http.post(
      `${this.API_URL}/grievances/${grievanceId}/assign`,
      staffId
    )
  }

  getOfficersByDepartment(departmentId: number) {
    return this.http.get<{ id: number; fullName: string; email: string }[]>(
      `${this.API_URL}/departments/${departmentId}/officers`
    );
  }

  closeGrievance(grievanceId: number, closingRemarks: string) {
    return this.http.post(
      `${this.API_URL}/grievances/${grievanceId}/remarks`,
      JSON.stringify(closingRemarks),
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      }
    );
  }
}
