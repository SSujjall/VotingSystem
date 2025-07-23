import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { NgIf } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, NgIf],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  constructor(public auth: AuthService) {}

  logout() {
    this.auth.logout();
  }
}
