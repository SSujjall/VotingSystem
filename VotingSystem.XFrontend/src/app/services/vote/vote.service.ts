import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environment';
import { VoteRequest } from '../../models/DTOs/vote-request.dto';
import { ApiResponse } from '../../models/api-response.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VoteService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // :Observable<ApiResponse<VoteData>>  chai type safety ko lagi matra rakheko ho
  // Default ma http le auto observable return garcha
  vote(payload: VoteRequest): Observable<ApiResponse<VoteData>> {
    return this.http.post<ApiResponse<VoteData>>(
      `${this.baseUrl}/Voting/vote`,
      payload
    );
  }

  removeVote(pollId: number) {
    return this.http.delete<ApiResponse<null>>(
      `${this.baseUrl}/Voting/remove-vote/${pollId}`
    );
  }

  getUserVote(pollId: number) {
    return this.http.get<ApiResponse<UserVoteData>>(
      `${this.baseUrl}/Voting/get-user-vote/${pollId}`
    );
  }
}
