import { Injectable } from '@angular/core';
import { environment } from '../../../../environment';
import { HttpClient } from '@angular/common/http';
import { HealthResponse } from '../../models/health-response.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HealthService {
  baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getHealthStatus(): Observable<HealthResponse> {
    return this.http.get<HealthResponse>(`${this.baseUrl}/health`);
  }
}
