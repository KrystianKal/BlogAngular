import { HttpClient } from '@angular/common/http';
import {computed, inject, Injectable} from '@angular/core';
import {catchError, of} from 'rxjs';
import { Profile } from './models/profile.model';
import {rxResource} from '@angular/core/rxjs-interop';

export type AuthUser = Profile | undefined;
export type Credentials = {
  username?: string;
  email: string;
  password: string;
};
@Injectable({ providedIn: 'root'})
export class AuthService {
  private http: HttpClient = inject(HttpClient);

  private authResource = rxResource({
    loader: () => this.http.get<Profile>('api/profiles').pipe(
      catchError(() => of(undefined)),
    )
  });

  //current user
  user = computed(() => this.authResource.value());
  error = this.authResource.error ;
  isLoading = this.authResource.isLoading;

  login = (user: Credentials) =>
    this.http.post<AuthUser>('api/users/login', { user });
  register = (user: Credentials) =>
    this.http.post('api/users', { user });

  logout = () => this.http.post('api/users/logout', {});
}
