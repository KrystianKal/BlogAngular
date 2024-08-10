import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { AuthorHeaderComponent } from '../author-header/author-header.component';
import { Article } from '../shared/models/article.model';
import { RouterLink } from '@angular/router';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-article-preview',
  standalone: true,
  imports: [MatCardModule, RouterLink, MatChipsModule, AuthorHeaderComponent],
  templateUrl: './article-preview.component.html',
})
export class ArticlePreviewComponent {
  @Input({ required: true })
  article!: Article;
}
