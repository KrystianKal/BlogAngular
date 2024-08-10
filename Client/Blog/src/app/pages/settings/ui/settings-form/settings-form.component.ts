import { Component, inject, input, signal } from '@angular/core';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {
  getErrorMessage,
  validImageUrl,
} from '../../../../shared/utils/form-validation.utils';
import { NgIf } from '@angular/common';
import { Profile } from '../../../../shared/models/profile.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProfileService } from '../../../article/ui/article-socials/profile.service';
import { catchError, Observable, of, switchMap } from 'rxjs';
import { AvatarComponent } from '../../../../avatar/avatar.component';

@Component({
  selector: 'app-settings-form',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    MatInputModule,
    MatButtonModule,
    AvatarComponent,
  ],
  templateUrl: './settings-form.component.html',
})
export class SettingsFormComponent {
  currentProfile = input.required<Profile>();
  getError = getErrorMessage;

  private snackBar = inject(MatSnackBar);
  private profileService = inject(ProfileService);
  fb = inject(FormBuilder);

  selectedFile = signal<File | null>(null);
  isUploading = signal<boolean>(false);

  settingsFrom = this.fb.group({
    imageUrl: ['', [validImageUrl()]],
    imageFile: ['', []],
    profileName: ['', [Validators.required]],
    bio: ['', Validators.required],
  });

  ngOnInit() {
    this.settingsFrom.controls.profileName.setValue(this.currentProfile().name);
    this.settingsFrom.controls.bio.setValue(this.currentProfile().bio);
  }

  //on chaning file url show preview and check if valid
  onSubmit() {
    const profileName = this.settingsFrom.controls.profileName.value;
    const bio = this.settingsFrom.controls.bio.value;
    const imageUrl = this.settingsFrom.controls.imageUrl.value;
    const request = { name: profileName!, bio: bio! };

    //waits for the image to finish uploading
    let uploadObservable: Observable<object | null> = of(null);

    if (this.selectedFile()) {
      const formData = new FormData();
      formData.append('file', this.selectedFile()!);

      uploadObservable = this.profileService.uploadPicture(formData).pipe(
        catchError((err) => {
          console.log(err);
          return of(null);
        })
      );
    }

    uploadObservable
      .pipe(
        switchMap(() => {
          const requestData =
            this.selectedFile() || !imageUrl
              ? request
              : { ...request, image: imageUrl };
          return this.profileService.update(requestData);
        })
      )
      .subscribe(() => {
        this.snackBar.open('Profile updated. Realoding...');
        window.location.reload();
      });
  }
  onFileSelected(event: any) {
    const files = event.target.files;
    if (files.length !== 1) {
      this.selectedFile.set(null);
      this.settingsFrom.controls.imageUrl.enable();
      return;
    }
    this.selectedFile.set(files[0]);
    this.settingsFrom.controls.imageUrl.disable();
  }
}
