import { Component, computed, inject, signal } from '@angular/core';
import { ProfileService } from '../article/ui/article-socials/profile.service';
import { Profile } from '../../shared/models/profile.model';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgIf } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FollowComponent } from '../../follow/follow.component';
import { ProfileTabsComponent } from './ui/profile-tabs/profile-tabs.component';
import { AvatarComponent } from '../../avatar/avatar.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    NgIf,
    MatProgressSpinnerModule,
    FollowComponent,
    ProfileTabsComponent,
    AvatarComponent,
  ],
  templateUrl: './profile.component.html',
  styles: [
    `
      .profile-avatar::after,
      .profile-avatar::before {
        content: '';
        position: absolute;
        height: 100%;
        width: 100%;
        background-image: repeating-linear-gradient(
            315deg,
            #00ffff2e 92%,
            #073aff00 100%
          ),
          repeating-radial-gradient(
            75% 75% at 238% 218%,
            #00ffff12 30%,
            #073aff14 39%
          ),
          repeating-conic-gradient(
            from 47deg at 109% 2%,
            #ff0000ff 0%,
            #073aff00 100%
          ),
          radial-gradient(99% 99% at 21% 78%, #7b00ffff 0%, #073aff00 100%),
          radial-gradient(
            160% 154% at 711px -303px,
            #2000ffff 0%,
            #073affff 100%
          );
        top: 50%;
        translate: 0% -50%;
        z-index: -1;
        border-radius: 10px;
      }
      .profile-avatar::before {
        filter: blur(1.5rem);
        opacity: 0.6;
      }
    `,
  ],
})
export class ProfileComponent {
  private profileService = inject(ProfileService);
  private route = inject(ActivatedRoute);

  profile = signal<Profile | null>(null);
  username = signal<string | null>(null);

  constructor() {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const username = params.get('username');
          this.username.set(username);
          return username
            ? this.profileService.get(username!)
            : this.profileService.getCurrent();
        })
      )
      .pipe(takeUntilDestroyed())
      .subscribe((profile) => this.profile.set(profile));
  }
}
