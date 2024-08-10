import { computed, inject, Injectable, signal } from '@angular/core';
import { AuthService, Credentials } from '../../../shared/auth.service';
import { catchError, EMPTY, Subject, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export type RegistrationStatus = 'pending' | 'creating' | 'success' | 'error';
interface RegistrationState {
  status: RegistrationStatus;
}
@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private authService = inject(AuthService);

  //data source
  error$ = new Subject<any>();
  createUser$ = new Subject<Credentials>();

  userCreated$ = this.createUser$.pipe(
    switchMap((credentials) =>
      this.authService.register(credentials).pipe(
        catchError((error) => {
          this.error$.next(error);
          return EMPTY;
        })
      )
    )
  );

  private state = signal<RegistrationState>({
    status: 'pending',
  });

  public status = computed(() => this.state().status);

  constructor() {
    this.userCreated$
      .pipe(takeUntilDestroyed())
      .subscribe((state) =>
        this.state.update((state) => ({ ...state, status: 'success' }))
      );

    this.createUser$
      .pipe(takeUntilDestroyed())
      .subscribe((state) =>
        this.state.update((state) => ({ ...state, status: 'creating' }))
      );

    this.error$
      .pipe(takeUntilDestroyed())
      .subscribe((state) =>
        this.state.update((state) => ({ ...state, status: 'error' }))
      );
  }
}
