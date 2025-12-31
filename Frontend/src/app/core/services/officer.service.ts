import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  OfficerGrievance,
  UpdateGrievanceStatusRequest
} from '../../shared/models/officer-grievance.model';

@Injectable({
  providedIn: 'root'
})
export class OfficerService {

  private readonly API_URL = 'http://localhost:5280/api/officer/grievances';

  constructor(private http: HttpClient) {}

  getAssignedGrievances() {
    return this.http.get<OfficerGrievance[]>(this.API_URL);
  }

  updateStatus(id: number, data: UpdateGrievanceStatusRequest) {
    return this.http.put(`${this.API_URL}/${id}/status`, data);
  }
}
