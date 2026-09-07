import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const token = localStorage.getItem('raras_token');

  let authReq = req;
  // Automatically attach Bearer token to API HTTP requests
  if (token && req.url.includes('/api/')) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Automatically handle 401 Unauthorized errors by clearing session & redirecting to login
      if (error.status === 401 && !req.url.includes('/api/auth/login')) {
        localStorage.removeItem('raras_token');
        localStorage.removeItem('raras_user');
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
