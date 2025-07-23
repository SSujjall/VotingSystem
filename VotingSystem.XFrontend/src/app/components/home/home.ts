import { Component, OnInit } from '@angular/core';
import { PollService } from '../../services/poll/poll.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  polls: any[] = [];
  loading = false;
  error = '';

  constructor(private pollService: PollService, private router: Router) {}

  ngOnInit() {
    this.loadActivePolls();
  }

  loadActivePolls() {
    this.loading = true;
    this.pollService.getAllPolls().subscribe({
      next: (res) => {
        this.polls = (res.data ?? []).filter((p) => p.isActive); // only active polls
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load polls';
        this.loading = false;
      },
    });
  }

  openPollVote(pollId: number) {
    this.router.navigate(['/poll-vote', pollId]);
  }
}
