import { computed, inject, Injectable, signal } from '@angular/core';
import { Comment } from '../../../shared/models/comment.model';
import { HttpClient } from '@angular/common/http';
import {
  catchError,
  delay,
  map,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

type CommentsResponse = {
  comments: Comment[];
};

type CommentsState = {
  slug: string | undefined;
  isLoading: boolean;
  comments: Comment[];
  error: string | null;
};

@Injectable({
  providedIn: 'root',
})
//redux-like pattern
export class CommentService {
  private http = inject(HttpClient);

  //state
  private state = signal<CommentsState>({
    slug: undefined,
    isLoading: false,
    comments: [],
    error: null,
  });

  //selectors
  private slug = computed(() => this.state().slug);
  isLoading = computed(() => this.state().isLoading);
  comments = computed(() => this.state().comments);
  error = computed(() => this.state().error);

  //sources
  private slugSubject = new Subject<string>();
  private createSubject = new Subject<string>();
  private deleteSubject = new Subject<string>();

  //reducers
  constructor() {
    this.slugSubject
      .pipe(
        tap((slug) => {
          this.setLoading(true);
          this.setSlug(slug);
        }),
        switchMap((slug) => this.getComments(slug)),
        delay(200),
        takeUntilDestroyed()
      )
      .subscribe((comments) => this.setStateComments(comments));

    this.createSubject
      .pipe(
        switchMap((body) => this.createComment(this.slug()!, body)),
        takeUntilDestroyed()
      )
      .subscribe((comment) => this.addStateComment(comment));

    this.deleteSubject
      .pipe(
        switchMap((id) => this.deleteComment(this.slug()!, id)),
        takeUntilDestroyed()
      )
      .subscribe((id) => this.removeStateComment(id));
  }

  private getComments(slug: string): Observable<Comment[]> {
    return this.http
      .get<CommentsResponse>(`api/articles/${slug}/comments`)
      .pipe(
        map((res) => res.comments),
        catchError((error) => this.setError(error))
      );
  }
  get(slug: string) {
    this.slugSubject.next(slug);
  }
  private deleteComment(slug: string, id: string): Observable<string> {
    return this.http
      .delete(`api/articles/${slug}/comments/${id}`)
      .pipe(map((res) => id));
  }
  delete(id: string) {
    this.deleteSubject.next(id);
  }
  private createComment(slug: string, body: string): Observable<Comment> {
    return this.http.post<Comment>(`api/articles/${slug}/comments`, {
      body,
    });
  }
  create(body: string) {
    this.createSubject.next(body);
  }

  private setLoading(isLoading: boolean) {
    this.state.update((state) => ({
      ...state,
      isLoading: isLoading,
    }));
  }
  private setSlug(slug: string) {
    this.state.update((state) => ({
      ...state,
      slug: slug,
    }));
  }
  private setStateComments(comments: Comment[]): void {
    this.state.update((state) => ({
      ...state,
      comments: comments,
      isLoading: false,
    }));
  }
  private addStateComment(comment: Comment): void {
    this.state.update((state) => ({
      ...state,
      comments: [comment, ...this.comments()],
    }));
  }

  private removeStateComment(id: string): void {
    this.state.update((state) => ({
      ...state,
      comments: [...this.comments().filter((comment) => comment.id !== id)],
    }));
  }
  private setError(error: string): Observable<Comment[]> {
    this.state.update((state) => ({
      ...state,
      error: error,
    }));
    return of([]);
  }
}
