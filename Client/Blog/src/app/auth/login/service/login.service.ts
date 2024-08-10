import { computed, inject, Injectable, signal } from '@angular/core';
import { AuthService, Credentials } from '../../../shared/auth.service';
import { catchError, EMPTY, Subject, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export type LoginStatus = 'pending' | 'authenticating' | 'success' | 'error';
interface LoginState {
  status: LoginStatus;
}
@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private authService: AuthService = inject(AuthService);

  //data source
  login$ = new Subject<Credentials>();
  error$ = new Subject();

  userAuthenticated$ = this.login$.pipe(
    switchMap((credentials) =>
      this.authService.login(credentials).pipe(
        catchError((err) => {
          this.error$.next(err);
          return EMPTY;
        })
      )
    )
  );

  private state = signal<LoginState>({
    status: 'pending',
  });

  status = computed(() => this.state().status);
  constructor() {
    this.userAuthenticated$
      .pipe(takeUntilDestroyed())
      .subscribe(() =>
        this.state.update((state) => ({ ...state, status: 'success' }))
      );

    this.login$
      .pipe(takeUntilDestroyed())
      .subscribe(() =>
        this.state.update((state) => ({ ...state, status: 'authenticating' }))
      );

    this.error$
      .pipe(takeUntilDestroyed())
      .subscribe(() =>
        this.state.update((state) => ({ ...state, status: 'error' }))
      );
  }
}
