import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Subject, switchMap } from 'rxjs';
import { AuthService } from '../../../../shared/auth.service';
import { Profile } from '../../../../shared/models/profile.model';

export type UpdateProfileRequest = {
  name: string;
  bio: string;
  imageUrl?: string;
};
@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  http = inject(HttpClient);

  constructor() {}

  get(username: string) {
    return this.http.get<Profile>(`api/profiles/${username}`);
  }
  getCurrent() {
    return this.http.get<Profile>(`api/profiles`);
  }

  follow(username: string) {
    return this.http.post<Profile>(`api/profiles/${username}/follow`, {});
  }
  unfollow(username: string) {
    return this.http.delete<Profile>(`api/profiles/${username}/follow`);
  }
  uploadPicture(formData: FormData) {
    return this.http.post('/api/profiles/picture', formData);
  }

  update(request: UpdateProfileRequest) {
    return this.http.patch('/api/profiles/', request);
  }
}
