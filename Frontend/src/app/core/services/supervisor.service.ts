import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Injectable({
  providedIn: 'root'
})
export class SupervisorService {

  private readonly API_URL =
    'http://localhost:5280/api/supervisor/grievances';

  constructor(private http: HttpClient) {}

  getOverdue(days: number) {
    return this.http.get<SupervisorGrievance[]>(
      `${this.API_URL}/overdue?days=${days}`
    );
  }
}
