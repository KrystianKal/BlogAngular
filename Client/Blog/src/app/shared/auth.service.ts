import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, Observable, of, switchMap, tap } from 'rxjs';
import { Profile } from './models/profile.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export type AuthUser = Profile | null | undefined;
export type Credentials = {
  username?: string;
  email: string;
  password: string;
};

interface AuthState {
  user: AuthUser;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);

  //data source
  private user$ = this.http.get<AuthUser>('/api/profiles').pipe(
    catchError((error: HttpErrorResponse) => {
      return of(null);
    })
  );

  //state
  private state = signal<AuthState>({
    user: undefined,
  });

  //selector
  user = computed(() => this.state().user);

  constructor() {
    this.user$.pipe(takeUntilDestroyed()).subscribe((user) =>
      this.state.update((state) => ({
        ...state,
        user,
      }))
    );
  }

  login(user: Credentials) {
    return this.http.post<AuthUser>('api/users/login', { user });
  }
  register(user: Credentials) {
    return this.http.post('api/users', { user });
  }

  logout() {
    return this.http.post('api/users/logout', {});
  }
}
