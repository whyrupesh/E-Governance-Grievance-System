import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateGrievanceRequest, Grievance } from '../../shared/models/grievance.model';
import { Category } from '../../shared/models/admin.model';

@Injectable({
  providedIn: 'root'
})
export class GrievanceService {

  private readonly API_URL = 'http://localhost:5280/api/grievances';

  constructor(private http: HttpClient) { }

  createGrievance(data: CreateGrievanceRequest) {
    return this.http.post(this.API_URL, data);
  }

  getMyGrievances() {
    return this.http.get<Grievance[]>(`${this.API_URL}/my`);
  }

  getGrievanceById(id: number) {
    return this.http.get<Grievance>(`${this.API_URL}/${id}`);
  }


  getCategories() {
    return this.http.get<Category[]>(`${this.API_URL}/categories`);
  }

  deleteMyGrievance(id: number) {
    return this.http.delete(`${this.API_URL}/${id}`);
  }


  addFeedback(id: number, data: any) {
    return this.http.post(`${this.API_URL}/${id}/feedback`, data);
  }

  reopenGrievance(id: number) {
    return this.http.post(`${this.API_URL}/${id}/reopen`, {});
  }

  escalateGrievance(id: number) {
    return this.http.post(`${this.API_URL}/${id}/escalate`, {});
  }
}
