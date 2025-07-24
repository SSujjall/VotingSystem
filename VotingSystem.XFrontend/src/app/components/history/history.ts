import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HistoryService } from '../../services/history/history.service';

@Component({
  selector: 'app-history',
  imports: [CommonModule],
  templateUrl: './history.html',
  styleUrl: './history.css',
})
export class History {
  votingHistory: any[] = [];
  isLoading = false;
  error = '';

  constructor(private historyService: HistoryService) {}

  ngOnInit() {
    this.getVotingHistory();
  }

  getVotingHistory(): void {
    this.isLoading = true;
    this.historyService.getVotingHistory().subscribe({
      next: (response) => {
        if (response.status && response.data) {
          this.votingHistory = response.data;
        } else {
          this.error = response.message || 'Something went wrong';
        }
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to fetch voting history';
        this.isLoading = false;
      },
    });
  }
}
