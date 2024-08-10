import {
  Component,
  computed,
  effect,
  inject,
  Input,
  signal,
} from '@angular/core';
import { Profile } from '../../shared/models/profile.model';
import { HttpClient } from '@angular/common/http';
import { UntypedFormArray } from '@angular/forms';
import { AuthService, AuthUser } from '../../shared/auth.service';
import { delay, Subject, switchMap, tap } from 'rxjs';
import { use } from 'marked';
import { ProfileService } from '../article/ui/article-socials/profile.service';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SettingsFormComponent } from './ui/settings-form/settings-form.component';

type SettingsState = {
  profile: Profile | undefined;
  isLoading: boolean;
};
@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [MatProgressSpinnerModule, SettingsFormComponent],
  templateUrl: './settings.component.html',
})
export class SettingsComponent {
  private authService = inject(AuthService);

  private state = signal<SettingsState>({
    profile: undefined,
    isLoading: true,
  });

  profile = computed(() => this.state().profile);
  isLoading = computed(() => this.state().isLoading);

  private user$ = toObservable(this.authService.user);

  constructor() {
    this.user$
      .pipe(
        tap(() =>
          this.state.update((state) => ({
            ...state,
            isLoading: true,
          }))
        ),
        takeUntilDestroyed()
      )
      .subscribe((authUser) => {
        if (authUser) {
          this.state.update((state) => ({
            ...state,
            isLoading: false,
            profile: authUser as Profile,
          }));
        }
      });
  }
}
