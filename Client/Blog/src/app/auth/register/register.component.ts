import { Component, effect, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {Credentials } from '../../shared/auth.service';
import { RegisterFormComponent } from './ui/register-form/register-form.component';
import { RegisterService } from './register.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginService } from '../login/login.service';

@Component({
    selector: 'app-register',
    imports: [RegisterFormComponent, RouterLink],
    templateUrl: './register.component.html'
})
export class RegisterComponent {
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  private credentials = signal<Credentials | null>(null);

  public registerService = inject(RegisterService);
  private loginService = inject(LoginService);

  constructor() {
    effect(() => {
      if (this.registerService.status() === 'success') {
        this.snackBar.open('Success! You will be redirected shortly.');
        setTimeout(() => {
          this.loginService.login$.next(this.credentials()!);
          this.router.navigateByUrl('/').then(() => window.location.reload());
        }, 1000);
      }
    });
  }

  onSubmit(event: Credentials) {
    this.registerService.createUser$.next(event);
    this.credentials.set(event);
  }
}
