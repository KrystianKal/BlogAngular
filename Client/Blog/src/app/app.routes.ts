import { Routes } from '@angular/router';
import { ArticlePreviewComponent } from './article-preview/article-preview.component';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ArticleComponent } from './pages/article/article.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { EditorComponent } from './pages/editor/editor.component';
import { isAuthenticatedGuard } from './shared/guards/auth.guard';
import { isNotAuthenticatedGuard } from './shared/guards/unauth.guard';

export const routes: Routes = [
  { path: 'article/:slug', component: ArticleComponent },
  { path: 'profile/:username', component: ProfileComponent },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [isAuthenticatedGuard],
  },
  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [isAuthenticatedGuard],
  },
  {
    path: 'edit',
    component: EditorComponent,
    canActivate: [isAuthenticatedGuard],
  },
  {
    path: 'edit/:slug',
    component: EditorComponent,
    canActivate: [isAuthenticatedGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [isNotAuthenticatedGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,

    canActivate: [isNotAuthenticatedGuard],
  },
  { path: '', component: HomeComponent },
];
