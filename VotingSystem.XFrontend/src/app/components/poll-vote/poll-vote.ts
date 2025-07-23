import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PollService } from '../../services/poll/poll.service';
import { VoteService } from '../../services/vote/vote.service';
import * as signalR from '@microsoft/signalr';
import { PollOption } from '../../models/poll.model';

@Component({
  selector: 'app-poll-vote',
  imports: [CommonModule],
  templateUrl: './poll-vote.html',
  styleUrl: './poll-vote.css',
})
export class PollVote implements OnInit, OnDestroy {
  pollId!: number;
  poll: any = null;
  userVoteOptionId?: number;
  loading = false;
  error = '';
  success = '';
  private hubConnection!: signalR.HubConnection;

  constructor(
    private route: ActivatedRoute,
    private pollService: PollService,
    private voteService: VoteService
  ) {}

  ngOnInit() {
    this.pollId = +this.route.snapshot.paramMap.get('id')!;
    this.loadPoll();
  }

  ngOnDestroy() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  loadPoll() {
    this.loading = true;
    this.pollService.getPollById(this.pollId).subscribe({
      next: (res) => {
        this.poll = res.data;
        this.loadUserVote();
        this.setupSignalR();
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load poll';
        this.loading = false;
      },
    });
  }

  // SIGNALR setup
  private setupSignalR() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7243/votehub', {
        withCredentials: false,
      })
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.hubConnection.invoke('JoinPollGroup', this.pollId.toString());
      })
      .catch((err) => console.error('Error connecting SignalR: ', err));

    this.hubConnection.on('ReceiveVoteUpdate', (data) => {
  if (data.pollId === this.pollId && this.poll?.options) {
    // Create a map from updated options for fast lookup
    const updatedVotesMap = new Map<number, number>();
    data.options.forEach((opt: PollOption) => {
      updatedVotesMap.set(opt.pollOptionId, opt.voteCount);
    });

    // Update voteCount for existing options, keep original order
    this.poll.options = this.poll.options.map((option: PollOption) => {
      const updatedCount = updatedVotesMap.get(option.pollOptionId);
      return updatedCount !== undefined
        ? { ...option, voteCount: updatedCount }
        : option;
    });
  }
});

  }

  loadUserVote() {
    this.voteService.getUserVote(this.pollId).subscribe({
      next: (res) => {
        this.userVoteOptionId = res.data?.pollOptionId;
      },
      error: () => {
        this.error = 'Failed to load user vote';
      },
    });
  }

  vote(optionId: number) {
    if (this.userVoteOptionId === optionId) return; // Already voted this

    this.voteService
      .vote({ pollId: this.pollId, pollOptionId: optionId })
      .subscribe({
        next: () => {
          this.userVoteOptionId = optionId;
          this.success = 'Vote submitted!';
          this.error = '';
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to submit vote';
          this.success = '';
        },
      });
  }

  clearVote() {
    this.voteService.removeVote(this.pollId).subscribe({
      next: () => {
        this.userVoteOptionId = undefined;
        this.success = 'Vote cleared';
        this.error = '';
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to clear vote';
        this.success = '';
      },
    });
  }
}
