import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PollService } from '../../../services/poll/poll.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  options: { value: string }[] = [];
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
        this.endsAt = poll.endsAt.split('T')[0];
        this.isActive = poll.isActive;
        this.options = poll.options.map((o: any) => ({ value: o.optionText }));
      },
      error: () => (this.error = 'Failed to load poll'),
    });
  }

  initEmptyPoll() {
    this.title = '';
    this.description = '';
    this.endsAt = new Date().toISOString().split('T')[0];
    this.isActive = false;
    this.options = [{ value: '' }];
  }

  addOption() {
    this.options.push({ value: '' });
  }

  removeOption(index: number) {
    this.options.splice(index, 1);
  }

  trackByIndex(index: number, item: any) {
    return index;
  }

  onSave() {
    this.error = '';
    this.success = '';

    const payload = {
      title: this.title,
      description: this.description,
      endsAt: new Date(this.endsAt).toISOString(),
      isActive: this.isActive,
      options: this.options.map((o) => o.value).filter((o) => o.trim() !== ''),
    };

    if (this.isEditMode) {
      this.pollService.updatePoll({ ...payload, pollId: this.pollId }).subscribe({
        next: () => (this.success = 'Poll updated successfully'),
        error: () => (this.error = 'Update failed'),
      });
    } else {
      this.pollService.addPoll(payload).subscribe({
        next: () => {
          this.success = 'Poll created successfully';
          this.router.navigate(['/dashboard']);
        },
        error: () => (this.error = 'Poll creation failed'),
      });
    }
  }

  enablePoll() {
    if (!this.isEditMode || this.isActive) return;
    this.pollService.enablePoll(this.pollId!).subscribe({
      next: () => {
        this.isActive = true;
        this.success = 'Poll enabled';
        this.error = '';
      },
      error: (err) =>
        (this.error = err?.error?.message || 'Enable failed'),
    });
  }

  disablePoll() {
    if (!this.isEditMode || !this.isActive) return;
    this.pollService.disablePoll(this.pollId!).subscribe({
      next: () => {
        this.isActive = false;
        this.success = 'Poll disabled';
        this.error = '';
      },
      error: (err) =>
        (this.error = err?.error?.message || 'Disable failed'),
    });
  }
}
