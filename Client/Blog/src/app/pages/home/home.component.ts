import { Component, effect, inject, signal } from '@angular/core';
import { ArticlePreviewComponent } from '../../article-preview/article-preview.component';
import { BehaviorSubject, map, Observable, switchMap } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { TagsComponent } from '../../tags/tags.component';
import {
  ArticlesService,
  ArticlesResponse,
} from '../../shared/articles.service';
import { MatTabChangeEvent, MatTabsModule } from '@angular/material/tabs';
import { PaginationComponent } from './ui/pagination/pagination.component';
import { AuthService } from '../../shared/auth.service';

interface ArticlesState {
  tag: string | null;
  tab: string;
  tabs: string[];
  limit: number;
  page: number;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    ArticlePreviewComponent,
    MatTabsModule,
    NgIf,
    AsyncPipe,
    MatProgressSpinner,
    TagsComponent,
    PaginationComponent,
  ],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private articleService = inject(ArticlesService);
  authService = inject(AuthService);

  private state$ = new BehaviorSubject<ArticlesState>({
    tag: null,
    tab: 'New',
    tabs: ['New'],
    page: 0,
    limit: 5,
  });

  articles$: Observable<ArticlesResponse> = this.state$.pipe(
    switchMap((state) => {
      return this.fetchArticles(state.tag, state.tab, state.page, state.limit);
    })
  );

  currentPage = this.state$.pipe(map((state) => state.page));
  limit = this.state$.pipe(map((state) => state.limit));
  tabs = this.state$.pipe(map((state) => state.tabs));

  constructor() {
    effect(() => {
      if (this.authService.user()) {
        this.state$.next({
          ...this.state$.value,
          tabs: [...this.state$.value.tabs, 'For You'],
        });
      }
    });
  }

  private fetchArticles(
    tag: string | null,
    tab: string,
    page: number,
    limit: number
  ) {
    if (tab === 'New') {
      return this.articleService.list({ tag: tag, page: page, limit: limit });
    }
    return this.articleService.feed({ page: page, limit: limit });
  }

  onTagChange(tag: string | null) {
    this.state$.next({ ...this.state$.value, tag: tag });
  }

  onPageChange($event: number) {
    this.state$.next({ ...this.state$.value, page: $event });
  }
  onTabChange(event: MatTabChangeEvent) {
    this.state$.next({
      ...this.state$.value,
      page: 0,
      tab: event.tab.textLabel,
    });
  }
}
