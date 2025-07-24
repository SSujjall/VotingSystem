import { Injectable } from '@angular/core';
import { environment } from '../../../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {
  baseUrl = environment.apiBaseUrl;

   constructor(private http: HttpClient) {}

  getVotingHistory(): Observable<ApiResponse<VoteHistoryResponse[]>> {
    return this.http.get<ApiResponse<VoteHistoryResponse[]>>(this.baseUrl + '/VotingHistory/get-history');
  }
}
