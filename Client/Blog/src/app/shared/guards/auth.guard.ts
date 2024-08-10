import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { inject } from '@angular/core';
import { filter, map, take, timeout } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { Profile } from '../models/profile.model';

export const isAuthenticatedGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return toObservable(authService.user).pipe(
    //wait for initial request if needed
    filter((user): user is Profile | null => user !== undefined),
    take(1),
    timeout(5000),
    map((user) => {
      if (user) {
        return true;
      } else {
        router.navigate(['/login']);
        return false;
      }
    })
  );
};
