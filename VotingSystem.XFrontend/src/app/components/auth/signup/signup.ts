import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiResponse } from '../../../models/api-response.model';

@Component({
  selector: 'app-signup',
  imports: [CommonModule, FormsModule],
  templateUrl: './signup.html',
  styleUrl: './signup.css'
})
export class Signup {
  username = '';
  password = '';
  fullName = '';
  email = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) { }

  onSignup() {
    this.auth.signup(this.username, this.fullName, this.email, this.password).subscribe({
      next: (res: ApiResponse<any>) => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error(err);
        if (err.error && err.error.errors && err.error.errors.Password) {
          this.error = err.error.errors.Password;
        } else if (err.error && err.error.errors && err.error.errors.Username) {
          this.error = err.error.errors.Username;
        }
        else if (err.error && err.error.message) {
          this.error = err.error.message;
        } else {
          this.error = 'Login failed. Please try again.';
        }
      }
    });
  }
}
