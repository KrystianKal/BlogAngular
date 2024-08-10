import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../shared/auth.service';
import { Router, RouterLink } from '@angular/router';
import { tap } from 'rxjs';
import { AvatarComponent } from '../../avatar/avatar.component';
import { MatIcon } from '@angular/material/icon';
import { NgClass, NgIf } from '@angular/common';
import { HeaderNavButtonComponent } from './header-nav-button/header-nav-button.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatButtonModule,
    RouterLink,
    NgClass,
    NgIf,
    MatIcon,
    HeaderNavButtonComponent,
    AvatarComponent,
  ],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  authService: AuthService = inject(AuthService);

  router = inject(Router);

  logout() {
    this.authService
      .logout()
      .pipe(
        tap(() => {
          window.location.reload();
        })
      )
      .subscribe();
  }
}
