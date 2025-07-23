import { HttpInterceptorFn, HttpRequest, HttpErrorResponse, HttpHandlerFn } from '@angular/common/http';
import { AuthService } from '../services/auth/auth.service';
import { inject } from '@angular/core';
import { catchError, tap, throwError } from 'rxjs';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.getToken();

  const cloned = token
    ? req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) })
    : req;

  return next(cloned).pipe(
    tap(res => {
      // Optional: success log/logic
    }),
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 || err.status === 403) {
        auth.logout();
      }
      return throwError(() => err);
    })
  );
};
