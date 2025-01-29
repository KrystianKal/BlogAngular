import { Component, effect, inject } from '@angular/core';
import { LoginFormComponent } from './ui/login-form/login-form.component';
import { LoginService } from './login.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterLink } from '@angular/router';

@Component({
    selector: 'app-login',
    imports: [LoginFormComponent, RouterLink, MatProgressSpinnerModule],
    templateUrl: './login.component.html'
})
export class LoginComponent {
  public loginService = inject(LoginService);
  private router = inject(Router);

  readonly userAuthenticatedEffect = effect(() => {
      if (this.loginService.status() === 'success') {
        this.router.navigateByUrl('/').then(() => {
          window.location.reload();
        });
      }
    });
}
