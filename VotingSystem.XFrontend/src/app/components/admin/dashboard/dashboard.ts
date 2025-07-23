import { Component, OnInit } from '@angular/core';
import { PollService } from '../../../services/poll/poll.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  polls: any[] = [];
  constructor(private pollService: PollService, private router: Router) {}

  ngOnInit() {
    this.loadPolls();
  }

  loadPolls() {
    this.pollService.getAllPolls().subscribe({
      next: (res) => {
        this.polls = res.data || [];
      },
      error: (err) => {
        console.error('Failed to load polls:', err);
      },
    });
  }

  onAddPoll() {
    this.router.navigate(['/add-poll']);
  }

  onEditPoll(poll: any) {
    if (!poll.pollId) {
      console.error('Poll ID is undefined or null!');
      return;
    }

    this.router.navigate(['/edit-poll', poll.pollId]);
  }

  onDeletePoll(pollId: string) {
    if (confirm('Are you sure you want to delete this poll?')) {
      this.pollService.deletePoll(pollId).subscribe({
        next: () => this.loadPolls(),
        error: (err) => console.error('Delete failed', err),
      });
    }
  }
}
