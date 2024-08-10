import { Component, inject, input, output, signal } from '@angular/core';
import { Credentials } from '../../../../shared/auth.service';
import {
  AbstractControl,
  FormBuilder,
  NgForm,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { LoginStatus } from '../../service/login.service';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { getErrorMessage } from '../../../../shared/utils/form-validation.utils';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [
    MatFormField,
    MatLabel,
    MatIcon,
    MatProgressSpinnerModule,
    MatInputModule,
    MatButtonModule,
    MatError,
    ReactiveFormsModule,
  ],
  templateUrl: './login-form.component.html',
})
export class LoginFormComponent {
  fb = inject(FormBuilder);

  loginStatus = input.required<LoginStatus>();
  login = output<Credentials>();

  loginForm = this.fb.nonNullable.group({
    email: ['user@mail.com', [Validators.email, Validators.required]],
    password: ['user', Validators.required],
  });
  onSubmit() {
    this.login.emit(this.loginForm.getRawValue());
  }

  getError = getErrorMessage;
}
