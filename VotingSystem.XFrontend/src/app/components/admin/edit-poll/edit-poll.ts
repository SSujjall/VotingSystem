import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PollService } from '../../../services/poll/poll.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Poll } from '../../../models/poll.model';

@Component({
  selector: 'app-edit-poll',
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-poll.html',
  styleUrl: './edit-poll.css',
  standalone: true,
})
export class EditPoll {
  pollId?: number;
  title = '';
  description = '';
  endsAt = '';
  isActive = false;
  options: string[] = [];
  error = '';
  success = '';
  isEditMode = false;
  headerTitle = 'Add Poll';

  constructor(
    private route: ActivatedRoute,
    private pollService: PollService,
    private router: Router
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.pollId = +idParam;
      this.isEditMode = true;
      this.headerTitle = 'Edit Poll';
      this.loadPoll();
    } else {
      this.isEditMode = false;
      this.headerTitle = 'Add Poll';
      this.initEmptyPoll();
    }
  }

  loadPoll() {
    this.pollService.getPollById(this.pollId!).subscribe({
      next: (res) => {
        const poll = res.data;
        if (!poll) {
          this.error = 'Poll not found';
          return;
        }
        this.title = poll.title;
        this.description = poll.description;
        this.endsAt = poll.endsAt.split('T')[0]; // for date input, keep only yyyy-MM-dd
        this.isActive = poll.isActive;
        this.options = poll.options.map((o: any) => o.optionText);
      },
      error: (err) => (this.error = 'Failed to load poll'),
    });
  }

  initEmptyPoll() {
    this.title = '';
    this.description = '';
    this.endsAt = new Date().toISOString().split('T')[0]; // default to today
    this.isActive = false;
    this.options = ['']; // start with one empty option
  }

  addOption() {
    this.options.push('');
  }

  removeOption(index: number) {
    this.options.splice(index, 1);
  }

  onSave() {
    this.error = '';
    this.success = '';

    const payload = {
      title: this.title,
      description: this.description,
      endsAt: new Date(this.endsAt).toISOString(),
      isActive: this.isActive,
      options: this.options.filter((o) => o.trim() !== ''),
    };

    if (this.isEditMode) {
      // For edit, include pollId
      this.pollService
        .updatePoll({ ...payload, pollId: this.pollId })
        .subscribe({
          next: () => (this.success = 'Poll updated successfully'),
          error: () => (this.error = 'Update failed'),
        });
    } else {
      // For add, call create endpoint
      this.pollService.addPoll(payload).subscribe({
        next: () => {
          this.success = 'Poll created successfully';
          this.router.navigate(['/dashboard']); // navigate back or reset form
        },
        error: () => (this.error = 'Poll creation failed'),
      });
    }
  }

  enablePoll() {
    if (!this.isEditMode || this.isActive) return; // Enable only in edit mode
    this.pollService.enablePoll(this.pollId!).subscribe({
      next: () => {
        this.isActive = true;
        this.success = 'Poll enabled';
        this.error = ''; // clear any previous error
      },
      error: (err) => {
        if (err?.error?.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Enable failed';
        }
      },
    });
  }

  disablePoll() {
    if (!this.isEditMode || !this.isActive) return; // Disable only in edit mode
    this.pollService.disablePoll(this.pollId!).subscribe({
      next: () => {
        this.isActive = false;
        this.success = 'Poll disabled';
        this.error = ''; // clear any previous error
      },
      error: (err) => {
        if (err?.error?.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Enable failed';
        }
      },
    });
  }
}
