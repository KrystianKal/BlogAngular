import {
  Component,
  computed,
  effect,
  inject,
  input,
  signal,
} from '@angular/core';
import { AuthService } from '../shared/auth.service';
import { ProfileService } from '../pages/article/ui/article-socials/profile.service';
import { Profile } from '../shared/models/profile.model';
import { NgClass, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-follow',
  standalone: true,
  imports: [NgIf, MatIconModule, NgClass],
  templateUrl: './follow.component.html',
})
export class FollowComponent {
  username = input.required<string>();

  private profileService = inject(ProfileService);
  private router = inject(Router);
  authService = inject(AuthService);

  private profile = signal<Profile | null>(null);

  isFollowed = computed(() => this.profile()?.following ?? false);
  profileName = computed(() => this.profile()?.name);

  constructor() {
    effect(() =>
      this.profileService
        .get(this.username())
        .subscribe((res) => this.profile.set(res))
    );
  }

  follow() {
    if (!this.authService.user()) {
      this.router.navigate(['/login']);
      return;
    }
    this.profileService.follow(this.username()).subscribe((profile) => {
      this.profile.set(profile);
    });
  }
  unfollow() {
    if (!this.authService.user()) {
      this.router.navigate(['/login']);
      return;
    }
    this.profileService.unfollow(this.username()).subscribe((profile) => {
      this.profile.set(profile);
    });
  }
}
