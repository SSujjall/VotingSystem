import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PollService } from '../../services/poll/poll.service';
import { VoteService } from '../../services/vote/vote.service';

@Component({
  selector: 'app-poll-vote',
  imports: [CommonModule],
  templateUrl: './poll-vote.html',
  styleUrl: './poll-vote.css',
})
export class PollVote implements OnInit {
  pollId!: number;
  poll: any = null;
  userVoteOptionId?: number;
  loading = false;
  error = '';
  success = '';

  constructor(
    private route: ActivatedRoute,
    private pollService: PollService,
    private voteService: VoteService
  ) {}

  ngOnInit() {
    this.pollId = +this.route.snapshot.paramMap.get('id')!;
    this.loadPoll();
  }

  loadPoll() {
    this.loading = true;
    this.pollService.getPollById(this.pollId).subscribe({
      next: (res) => {
        this.poll = res.data;
        this.loadUserVote();
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load poll';
        this.loading = false;
      },
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
