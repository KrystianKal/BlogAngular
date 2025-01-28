import {
  Component,
  computed,
  effect,
  inject,
  input,
  signal,
} from '@angular/core';
import { AuthService } from '../../../shared/auth.service';
import { ProfileService } from '../profile.service';
import { Profile } from '../../../shared/models/profile.model';
import { NgClass, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { rxResource } from '@angular/core/rxjs-interop';
import {Observable} from "rxjs";

@Component({
  selector: 'app-follow',
  imports: [NgIf, MatIconModule, NgClass],
  templateUrl: './follow.component.html'
})
export class FollowComponent {
  username = input.required<string>();

  private profileService = inject(ProfileService);
  private router = inject(Router);
  private authService = inject(AuthService);

  private profileResource = rxResource({
    loader: () => this.profileService.get(this.username())
  })
  isFollowed = computed(() => this.profileResource.value()?.following ?? false);
  profileName = computed(() => this.profileResource.value()?.name);
  currentUserIsNotTheOwner = computed(() =>
    this.authService.user()?.name !== this.profileName()
  )

  follow = () =>
    this.handleFollowAction(this.profileService.follow(this.username()));
  unfollow = () =>
    this.handleFollowAction(this.profileService.unfollow(this.username()));
  private handleFollowAction(action: Observable<Profile>){
    if (!this.authService.user()){
      this.router.navigate(['/login']);
      return;
    }
    action.subscribe(profile => {this.profileResource.set(profile)});
  }
}
