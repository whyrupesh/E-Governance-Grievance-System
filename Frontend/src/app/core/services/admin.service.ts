import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Department, Category, Officer } from '../../shared/models/admin.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private readonly API_URL = 'http://localhost:5280/api/admin';

  constructor(private http: HttpClient) { }

  // Departments
  getDepartments() {
    return this.http.get<Department[]>(`${this.API_URL}/departments`);
  }

  addDepartment(name: string, description : string) {
    return this.http.post(`${this.API_URL}/departments`, { name, description });
  }

  // Categories
  getCategories() {
    return this.http.get<Category[]>(`${this.API_URL}/categories`);
  }

  addCategory(data: { name: string; departmentId: number }) {
    return this.http.post(`${this.API_URL}/categories`, data);
  }

  // Officers
  getOfficers() {
    return this.http.get<Officer[]>(`${this.API_URL}/officers`);
  }

  createOfficer(data: any) {
    return this.http.post(`${this.API_URL}/officers`, data);
  }
}
