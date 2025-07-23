import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UpdateUserDto } from '../../models/DTOs/update-user.dto';
import { UserProfileService } from '../../services/userProfile/user-profile.service';

@Component({
  selector: 'app-user-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  fullName = '';
  username = '';
  email = '';

  loading = false;
  success = '';
  error = '';

  constructor(private userService: UserProfileService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    this.loading = true;
    this.userService.getUser().subscribe({
      next: (res) => {
        if (res.status && res.data) {
          this.fullName = res.data.fullName;
          this.username = res.data.username;
          this.email = res.data.email;
        } else {
          this.error = 'Failed to load user';
        }
        this.loading = false;
      },
      error: () => {
        this.error = 'Error fetching user data';
        this.loading = false;
      },
    });
  }

  onSubmit(form: any) {
    if (form.invalid) return;

    const updatedUser: UpdateUserDto = {
      fullName: this.fullName,
      username: this.username,
      email: this.email,
    };

    this.userService.updateUser(updatedUser).subscribe({
      next: (res) => {
        if (res.status) {
          this.success = 'Profile updated successfully';
          this.error = '';
        } else {
          this.error = 'Failed to update profile';
          this.success = '';
        }
      },
      error: () => {
        this.error = 'Update failed';
        this.success = '';
      },
    });
  }
}
