import { Injectable } from '@angular/core';
import { ApiResponse } from '../../models/api-response.model';
import { environment } from '../../../../environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Poll } from '../../models/poll.model';
import { CreatePollDto } from '../../models/DTOs/create-poll.dto';

@Injectable({
  providedIn: 'root',
})
export class PollService {
  private baseUrl = environment.apiBaseUrl;
  constructor(private http: HttpClient, private router: Router) {}

  addPoll(payload: CreatePollDto) {
    return this.http.post<ApiResponse<Poll>>(`${this.baseUrl}/Polls/create`, payload);
  }

  getAllPolls() {
    return this.http.get<ApiResponse<Poll[]>>(`${this.baseUrl}/Polls/get-all`);
  }

  getPollById(id: number) {
    return this.http.get<ApiResponse<Poll>>(
      `${this.baseUrl}/Polls/get-by-id/${id}`
    );
  }

  updatePoll(payload: any) {
    return this.http.put(
      `${this.baseUrl}/Polls/update`,
      payload
    );
  }

  enablePoll(id: number) {
    return this.http.patch(`${this.baseUrl}/Polls/enable/${id}`, {});
  }

  disablePoll(id: number) {
    return this.http.patch(`${this.baseUrl}/Polls/disable/${id}`, {});
  }

  deletePoll(id: string) {
    return this.http.delete<ApiResponse<null>>(`${this.baseUrl}/Polls/delete/${id}`);
  }
}
