import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const auth = inject(AuthService);

  const token = auth.getToken();
  if (!token || !auth.isLoggedIn()) {
    return router.parseUrl('/login');
  }

  const role = auth.getUserRole();
  if (role === 'Admin' || role === 'Superadmin') {
    return true;
  }

  return router.parseUrl('/unauthorized');
};
