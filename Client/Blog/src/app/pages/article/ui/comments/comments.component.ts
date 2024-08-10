import { Component, inject, input } from '@angular/core';
import { Comment } from '../../../../shared/models/comment.model';
import { NgIf } from '@angular/common';
import { CommentComponent } from '../comment/comment.component';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommentService } from '../../services/comment.service';
import { MatButtonModule } from '@angular/material/button';
import { getErrorMessage } from '../../../../shared/utils/form-validation.utils';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../../../shared/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [
    MatFormFieldModule,
    NgIf,
    MatButtonModule,
    FormsModule,
    MatInputModule,
    RouterLink,
    CommentComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './comments.component.html',
})
export class CommentsComponent {
  slug = input.required<string>();

  getErrors = getErrorMessage;

  private fb = inject(FormBuilder);
  commentService = inject(CommentService);
  authService = inject(AuthService);

  comments = this.commentService.comments;
  isLoading = this.commentService.isLoading;

  //form
  commentForm = this.fb.group({
    body: ['', [Validators.required, Validators.maxLength(500)]],
  });

  ngOnInit() {
    this.commentService.get(this.slug());
  }

  submit() {
    if (this.commentForm.invalid) {
      return;
    }
    const body = this.commentForm.get('body')?.value;
    if (!body) {
      return;
    }
    this.commentService.create(body);
    this.commentForm.reset();
  }
}
