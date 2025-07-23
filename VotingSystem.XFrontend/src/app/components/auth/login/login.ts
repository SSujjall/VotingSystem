import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiResponse } from '../../../models/api-response.model';
import { LoginResponse } from '../../../models/login-response.model';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  standalone: true,
  styleUrl: './login.css',
})
export class Login {
  username = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  onLogin() {
    this.auth
      .login({ username: this.username, password: this.password })
      .subscribe({
        next: (res: ApiResponse<LoginResponse>) => {
          const token = res.data?.jwtToken;
          if (token) {
            this.auth.saveToken(token);
            const role = this.auth.getUserRole();

            console.log('User role:', role);

            if (
              role?.toUpperCase() === 'ADMIN' ||
              role?.toUpperCase() === 'SUPERADMIN'
            ) {
              this.router.navigate(['/dashboard']);
            } else {
              this.router.navigate(['/home']);
            }
          } else {
            this.error = 'Login failed. Invalid token received.';
          }
        },
        error: (err) => {
          if (err.error && err.error.errors && err.error.errors.Username) {
            this.error = err.error.errors.Username;
          } else if (
            err.error &&
            err.error.errors &&
            err.error.errors.Password
          ) {
            this.error = err.error.errors.Password;
          } else if (err.error && err.error.message) {
            this.error = err.error.message;
          } else {
            this.error = 'Login failed. Please try again.';
          }
        },
      });
  }
}
