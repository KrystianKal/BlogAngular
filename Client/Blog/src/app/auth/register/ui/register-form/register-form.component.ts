import { Component, inject, input, output } from '@angular/core';
import { Credentials } from '../../../../shared/auth.service';
import { RegistrationStatus } from '../../service/register.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import {
  getErrorMessage,
  noSpecialCharacters,
} from '../../../../shared/utils/form-validation.utils';

@Component({
  selector: 'app-register-form',
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
  templateUrl: './register-form.component.html',
})
export class RegisterFormComponent {
  private fb = inject(FormBuilder);

  registrationStatus = input.required<RegistrationStatus>();
  register = output<Credentials>();

  registerForm = this.fb.nonNullable.group({
    username: [
      '',
      [Validators.required, Validators.minLength(3), noSpecialCharacters()],
    ],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(3)]],
  });

  onSubmit() {
    this.register.emit(this.registerForm.getRawValue());
  }

  getError = getErrorMessage;
}
