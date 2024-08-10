import { Component, inject, Input } from '@angular/core';
import { AuthorHeaderComponent } from '../../author-header/author-header.component';
import { MatDivider } from '@angular/material/divider';
import { MatChip } from '@angular/material/chips';
import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { ArticleSocialsComponent } from './ui/article-socials/article-socials.component';
import { ArticleService } from '../../shared/article.service';
import { FollowComponent } from '../../follow/follow.component';
import { CommentsComponent } from './ui/comments/comments.component';
import { MarkedPipe } from '../../shared/utils/marked.pipe';
import { AuthService } from '../../shared/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-article',
  standalone: true,
  imports: [
    AuthorHeaderComponent,
    ArticleSocialsComponent,
    FollowComponent,
    CommentsComponent,
    NgIf,
    MatProgressSpinner,
    MatChip,
    NgClass,
    AsyncPipe,
    MatDivider,
    MarkedPipe,
  ],
  templateUrl: './article.component.html',
})
export class ArticleComponent {
  @Input('slug') slug = '';
  articleService = inject(ArticleService);
  authService = inject(AuthService);
  private router = inject(Router);

  article = this.articleService.article;
  status = this.articleService.status;

  ngOnInit() {
    this.articleService.get(this.slug);
  }

  toggleFavorite() {
    if (!this.authService.user()) {
      this.router.navigate(['/login']);
      return;
    }
    if (this.article()?.favorited) {
      this.articleService.unfavorite(this.article()!.slug);
    } else {
      this.articleService.favorite(this.article()!.slug);
    }
  }

  onDelete() {
    this.articleService.delete(this.article()!.slug);
    this.router.navigateByUrl('/');
  }
  onEdit() {
    this.router.navigateByUrl(`/edit/${this.slug}`);
  }
}
