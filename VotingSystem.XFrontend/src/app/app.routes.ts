import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { authGuard } from './guards/auth-guard';
import { Login } from './components/auth/login/login';
import { Signup } from './components/auth/signup/signup';
import { Dashboard } from './components/admin/dashboard/dashboard';
import { adminGuard } from './guards/admin/admin-guard';
import { EditPoll } from './components/admin/edit-poll/edit-poll';
import { PollVote } from './components/poll-vote/poll-vote';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'signup',
    component: Signup,
  },
  {
    path: 'home',
    component: Home,
    canActivate: [authGuard],
    title: 'Home',
  },
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [authGuard, adminGuard],
    title: 'Dashboard',
  },
  {
    path: 'add-poll',
    component: EditPoll,
    canActivate: [authGuard, adminGuard],
    title: 'Add Poll',
  },
  {
    path: 'edit-poll/:id',
    component: EditPoll,
    canActivate: [authGuard, adminGuard],
    title: 'Edit Poll',
  },
  {
    path: 'poll-vote/:id',
    component: PollVote,
    canActivate: [authGuard],
    title: 'Vote on Poll',
  },
];
