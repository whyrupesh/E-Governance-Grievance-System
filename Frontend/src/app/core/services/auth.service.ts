import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../shared/models/auth.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly API_URL = 'http://localhost:5280/api/auth'; 

  constructor(private http: HttpClient) {}

  login(data: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, data)
      .pipe(
        tap(res => this.storeAuth(res))
      );
  }

  register(data: RegisterRequest) {
    return this.http.post(`${this.API_URL}/register`, data);
  }

  private storeAuth(res: AuthResponse) {
    localStorage.setItem('token', res.token);
    localStorage.setItem('role', res.role);
    localStorage.setItem('user', JSON.stringify(res));
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  getUser(): any {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  hasRole(role: string): boolean {
  return this.getRole() === role;
}

  logout() {
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace('/login');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
