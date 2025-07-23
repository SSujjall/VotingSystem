import { Routes } from '@angular/router';
import { Home } from './components/home/home'
import { authGuard } from './guards/auth-guard';
import { Login } from './components/auth/login/login';
import { Signup } from './components/auth/signup/signup';

export const routes: Routes = [
    { 
        path: 'login', 
        component: Login 
    },
    { 
        path: 'signup', 
        component: Signup 
    },
    {
        path: 'home',
        component: Home,
        canActivate: [authGuard],
        title: "Home"
    },
];
