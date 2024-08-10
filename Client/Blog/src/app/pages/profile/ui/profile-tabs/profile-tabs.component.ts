import { Component, inject, input } from '@angular/core';
import { ArticlePreviewComponent } from '../../../../article-preview/article-preview.component';
import { MatTabChangeEvent, MatTabsModule } from '@angular/material/tabs';
import { AsyncPipe, NgIf } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { PaginationComponent } from '../../../home/ui/pagination/pagination.component';
import {
  ArticlesResponse,
  ArticlesService,
} from '../../../../shared/articles.service';
import { BehaviorSubject, map, Observable, switchMap } from 'rxjs';

type ProfileTab = 'Favorited' | 'Authored';

interface ArticlesState {
  tab: ProfileTab;
  limit: number;
  page: number;
}

@Component({
  selector: 'app-profile-tabs',
  standalone: true,
  imports: [
    ArticlePreviewComponent,
    MatTabsModule,
    NgIf,
    AsyncPipe,
    MatProgressSpinner,
    PaginationComponent,
  ],
  templateUrl: './profile-tabs.component.html',
})
export class ProfileTabsComponent {
  username = input.required<string>();

  private articleService = inject(ArticlesService);

  private state$ = new BehaviorSubject<ArticlesState>({
    tab: 'Favorited',
    page: 0,
    limit: 5,
  });

  currentPage = this.state$.pipe(map((state) => state.page));
  limit = this.state$.pipe(map((state) => state.limit));
  tabs: ProfileTab[] = ['Favorited', 'Authored'];

  articles$: Observable<ArticlesResponse> = this.state$.pipe(
    switchMap((state) => {
      return this.fetchArticles(state.tab, state.page, state.limit);
    })
  );
  private fetchArticles(tab: ProfileTab, page: number, limit: number) {
    switch (tab) {
      case 'Authored': {
        return this.articleService.list({
          author: this.username(),
          page: page,
          limit: limit,
        });
      }
      case 'Favorited': {
        return this.articleService.list({
          favorited: this.username(),
          page: page,
          limit: limit,
        });
      }
    }
  }

  onPageChange($event: number) {
    this.state$.next({ ...this.state$.value, page: $event });
  }

  onTabChange(event: string) {
    this.state$.next({
      ...this.state$.value,
      page: 0,
      tab: event as ProfileTab,
    });
  }
}
