import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import {computed,  inject} from '@angular/core';
import {catchError, filter, map, of} from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';

export const isAuthenticatedGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const authState = computed(() => ({
    user: authService.user(),
    loading: authService.isLoading(),
  }));

  return toObservable(authState).pipe(
    filter(({loading}) => !loading),
    map(({user}) => {
      if (user) {
        return true;
      } else {
        router.navigate(['/login']);
        return false;
      }
    }),
    catchError(() => {
      router.navigate(['/login']);
      return of(false);
    })
  );
};
