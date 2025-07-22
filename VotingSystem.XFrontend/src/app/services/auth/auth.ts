import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {environment} from '../../../../environment'

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient, private router: Router) { }

  login(credentials: { username: string; password: string }) {
    return this.http.post<any>(`${this.baseUrl}/login`, credentials);
  }
}
