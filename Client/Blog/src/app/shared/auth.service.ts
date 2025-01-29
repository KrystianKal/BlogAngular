import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {computed, effect, inject, Injectable, linkedSignal, signal} from '@angular/core';
import {catchError, map, Observable, of, switchMap, tap} from 'rxjs';
import { Profile } from './models/profile.model';
import {rxResource, takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Router} from "@angular/router";

export type AuthUser = Profile | null | undefined;
export type Credentials = {
  username?: string;
  email: string;
  password: string;
};
@Injectable({ providedIn: 'root'})
export class AuthService {
  private http: HttpClient = inject(HttpClient);
  private router = inject(Router);

  private authResource = rxResource({
    loader: () => this.http.get<Profile>('api/profiles').pipe(
      catchError(() => of(undefined)),
    )
  });

  user = computed(() => this.authResource.value());
  error = this.authResource.error ;
  isLoading = this.authResource.isLoading;
  private userSignal = effect(() =>
  {
    console.log("AuthResource:", this.authResource.value())
    console.log("IsLoading:", this.isLoading())
    console.log("User:", this.user())
  }
  )

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
