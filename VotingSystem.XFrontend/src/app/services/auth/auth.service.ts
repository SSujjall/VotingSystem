import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../environment'
import { ApiResponse } from '../../models/api-response.model';
import { LoginResponse } from '../../models/login-response.model';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiBaseUrl;
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private router: Router) { }
  

  login(credentials: { username: string; password: string }) {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.baseUrl}/Auth/login`, credentials);
  }

  signup(username: string, fullName: string, email: string, password: string) {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/Auth/signup`, { username, fullName, email, password });
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
    
    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
  }

  isAdmin(): boolean {
    const role = this.getUserRole();
    return role?.toUpperCase() === 'ADMIN' || role?.toUpperCase() === 'SUPERADMIN';
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}