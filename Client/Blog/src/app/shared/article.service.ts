import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Article } from './models/article.model';
import { Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export type ArticleStatus =
  | 'Created'
  | 'Edited'
  | 'Deleted'
  | 'Pending'
  | 'Loading'
  | 'Fetched'
  | 'Error';

type ArticleState = {
  article: Article | null;
  status: ArticleStatus;
};

export type CreateArticleRequest = Pick<
  Article,
  'title' | 'description' | 'body' | 'tagList'
>;
export type EditArticleRequest = Pick<
  Article,
  'title' | 'description' | 'body'
>;

@Injectable({
  providedIn: 'root',
})
export class ArticleService {
  http = inject(HttpClient);

  private state = signal<ArticleState>({
    article: null,
    status: 'Pending',
  });

  status = computed(() => this.state().status);
  article = computed(() => this.state().article);

  private getSubject = new Subject<string>();
  private createSubject = new Subject<CreateArticleRequest>();
  private editSubject = new Subject<EditArticleRequest>();
  private deleteSubject = new Subject<string>();
  private favoriteSubject = new Subject<string>();
  private unfavoriteSubject = new Subject<string>();

  constructor() {
    this.getSubject
      .pipe(
        tap(() =>
          this.state.update((state) => ({
            ...state,
            status: 'Loading',
          }))
        ),
        switchMap((slug) => this.getArticle(slug)),
        takeUntilDestroyed()
      )
      .subscribe((article) => {
        this.state.update((state) => ({
          ...state,
          article: article,
          status: 'Fetched',
        }));
      });

    this.createSubject
      .pipe(
        switchMap((createRequest) => this.createArticle(createRequest)),
        takeUntilDestroyed()
      )
      .subscribe((res) => {
        this.state.update((state) => ({
          ...state,
          article: res,
          status: 'Created',
        }));
      });

    this.editSubject
      .pipe(
        switchMap((editRequest) =>
          this.editArticle(this.article()!.slug, editRequest)
        ),
        takeUntilDestroyed()
      )
      .subscribe((res) =>
        this.state.update((state) => ({
          ...state,
          article: res,
          status: 'Edited',
        }))
      );

    this.deleteSubject
      .pipe(
        switchMap((editRequest) => this.deleteArticle(this.article()!.slug)),
        takeUntilDestroyed()
      )
      .subscribe((res) =>
        this.state.update((state) => ({
          ...state,
          article: null,
          status: 'Deleted',
        }))
      );

    this.favoriteSubject
      .pipe(
        switchMap((editRequest) => this.favoriteArticle(this.article()!.slug)),
        takeUntilDestroyed()
      )
      .subscribe((res) =>
        this.state.update((state) => ({
          ...state,
          article: res,
        }))
      );

    this.unfavoriteSubject
      .pipe(
        switchMap((editRequest) =>
          this.unfavoriteArticle(this.article()!.slug)
        ),
        takeUntilDestroyed()
      )
      .subscribe((res) =>
        this.state.update((state) => ({
          ...state,
          article: res,
        }))
      );
  }

  private getArticle(slug: string) {
    return this.http.get<Article>(`api/articles/${slug}`);
  }
  get(slug: string) {
    this.getSubject.next(slug);
  }
  private editArticle(slug: string, article: EditArticleRequest) {
    return this.http.put<Article>(`api/articles/${slug}`, { article: article });
  }
  edit(article: EditArticleRequest) {
    this.editSubject.next(article);
  }

  private deleteArticle(slug: string) {
    return this.http.delete(`api/articles/${slug}`);
  }
  delete(slug: string) {
    this.deleteSubject.next(slug);
  }

  private createArticle(article: CreateArticleRequest) {
    return this.http.post<Article>(`api/articles`, { article: article });
  }
  create(article: CreateArticleRequest) {
    this.createSubject.next(article);
  }

  private favoriteArticle(slug: string) {
    return this.http.post<Article>(`api/articles/${slug}/favorite`, {});
  }
  favorite(slug: string) {
    this.favoriteSubject.next(slug);
  }
  private unfavoriteArticle(slug: string) {
    return this.http.delete<Article>(`api/articles/${slug}/favorite`, {});
  }
  unfavorite(slug: string) {
    this.unfavoriteSubject.next(slug);
  }
}
