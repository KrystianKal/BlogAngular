import { DatePipe, NgIf } from '@angular/common';
import { Component, effect, Input, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { Article } from '../shared/models/article.model';
import { Author } from '../shared/models/author.model';
import { AvatarComponent } from '../avatar/avatar.component';

type CreatedAt =
  | {
      date: string;
      format?: string;
    }
  | undefined;
@Component({
  selector: 'app-author-header',
  standalone: true,
  imports: [
    MatCardModule,
    RouterLink,
    NgIf,
    AuthorHeaderComponent,
    DatePipe,
    AvatarComponent,
  ],
  templateUrl: './author-header.component.html',
})
export class AuthorHeaderComponent {
  author = input.required<Author>();
  @Input()
  createdAt: CreatedAt;
}
