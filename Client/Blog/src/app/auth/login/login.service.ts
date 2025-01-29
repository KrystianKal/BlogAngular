import {computed, effect, inject, Injectable, signal} from '@angular/core';
import { AuthService, Credentials } from '../../shared/auth.service';

export type LoginStatus = 'pending' | 'authenticating' | 'success' | 'error';
@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private authService: AuthService = inject(AuthService);

  private credentials = signal<Credentials | null>(null);
  private authState = signal<LoginStatus>('pending');
  status = computed(() => this.authState());

  readonly loginEffect = effect(() => {
    const creds = this.credentials();
    if(!creds) return;
    this.authState.set('authenticating');
    this.authService.login(creds).subscribe({
      next: () => this.authState.set('success'),
      error: (e) => this.authState.set('error'),
    })
  })
  login = (credentials: Credentials) => this.credentials.set(credentials);
}
