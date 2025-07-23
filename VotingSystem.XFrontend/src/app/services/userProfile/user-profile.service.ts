import { Injectable } from '@angular/core';
import { environment } from '../../../../environment';
import { Observable } from 'rxjs';
import { UpdateUserDto } from '../../models/DTOs/update-user.dto';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getUser(): Observable<ApiResponse<UpdateUserDto>> {
    return this.http.get<ApiResponse<UpdateUserDto>>(`${this.baseUrl}/User/get-user`);
  }

  updateUser(data: UpdateUserDto): Observable<ApiResponse<UpdateUserDto>> {
    return this.http.post<ApiResponse<UpdateUserDto>>(`${this.baseUrl}/User/update-user`, data);
  }
}
