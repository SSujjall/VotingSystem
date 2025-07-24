import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { authGuard } from './guards/auth-guard';
import { Login } from './components/auth/login/login';
import { Signup } from './components/auth/signup/signup';
import { Dashboard } from './components/admin/dashboard/dashboard';
import { adminGuard } from './guards/admin/admin-guard';
import { EditPoll } from './components/admin/edit-poll/edit-poll';
import { PollVote } from './components/poll-vote/poll-vote';
import { UserProfile } from './components/user-profile/user-profile';
import { redirectIfAuthGuard } from './guards/redirect/redirect-if-auth-guard';
import { Health } from './components/health/health';
import { History } from './components/history/history';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'login',
    component: Login,
    canActivate: [redirectIfAuthGuard],
  },
  {
    path: 'signup',
    component: Signup,
    canActivate: [redirectIfAuthGuard],
  },
  {
    path: 'home',
    component: Home,
    canActivate: [authGuard],
    title: 'Home',
  },
  {
    path: 'user-profile',
    component: UserProfile,
    canActivate: [authGuard],
    title: 'User Profile',
  },
  {
    path: 'history',
    component: History,
    canActivate: [authGuard],
    title: 'Voting History',
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
    path: 'health',
    component: Health,
    canActivate: [authGuard, adminGuard],
    title: 'Health',
  },
  {
    path: 'poll-vote/:id',
    component: PollVote,
    canActivate: [authGuard],
    title: 'Vote on Poll',
  },
];
