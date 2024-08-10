import { Component, inject, input, output } from '@angular/core';
import { Comment } from '../../../../shared/models/comment.model';
import { AuthorHeaderComponent } from '../../../../author-header/author-header.component';
import { AuthService } from '../../../../shared/auth.service';

@Component({
  selector: 'app-comment',
  standalone: true,
  imports: [AuthorHeaderComponent],
  template: `<div class="flex flex-col bg-slate-50  rounded ">
    <p class="p-4">
      {{ comment().body }}
    </p>
    <div
      class="border-t-2 p-2 bg-secondary flex flex-row justify-between items-center"
    >
      <app-author-header
        [author]="comment().author"
        [createdAt]="{ date: comment().createdAt, format: 'dd.MM.yyyy hh:mm' }"
      />
      @if(authService.user() && authService.user()!.name ===
      comment().author.profileName){
      <button
        class="border-danger border p-1 h-1/4 font-mono text-danger rounded"
        (click)="commentDeleted.emit(comment().id)"
      >
        Delete
      </button>
      }
    </div>
  </div> `,
})
export class CommentComponent {
  comment = input.required<Comment>();
  commentDeleted = output<string>();

  authService = inject(AuthService);
}
