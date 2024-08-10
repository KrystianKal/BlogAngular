import {
  Component,
  computed,
  effect,
  inject,
  Input,
  signal,
} from '@angular/core';
import {
  ArticleService,
  CreateArticleRequest,
  EditArticleRequest,
} from '../../shared/article.service';
import { Article } from '../../shared/models/article.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { EditorFormComponent } from './ui/editor-form/editor-form.component';
import { EditorPreviewComponent } from './ui/editor-preview/editor-preview.component';
import { MatDivider } from '@angular/material/divider';
import { AuthService } from '../../shared/auth.service';

export type EditorStatus = 'Creating' | 'Editing' | undefined;
export type ArticleInProgress = Pick<
  Article,
  'title' | 'description' | 'body' | 'tagList'
>;

interface EditorState {
  status: EditorStatus;
  articleInProgress: ArticleInProgress | undefined;
}

@Component({
  selector: 'app-editor',
  standalone: true,
  imports: [EditorFormComponent, MatDivider, EditorPreviewComponent],
  templateUrl: './editor.component.html',
})
export class EditorComponent {
  @Input('slug') slug = '';

  private articleService = inject(ArticleService);
  private authService = inject(AuthService);
  private router = inject(Router);

  state = signal<EditorState>({
    status: undefined,
    articleInProgress: { title: '', description: '', body: '', tagList: [] },
  });
  private article$ = toObservable(this.articleService.article);

  status = computed(() => this.state().status);
  article = computed(() => this.state().articleInProgress);
  isLoading = computed(() => this.articleService.status() === 'Loading');

  constructor() {
    effect(() => {
      this.article$.subscribe((article) => {
        if (['Created', 'Edited'].includes(this.articleService.status())) {
          this.router.navigateByUrl(`/article/${article?.slug}`);
        }
        let currentUserIsNotTheAuthor =
          this.authService.user() &&
          article &&
          this.slug &&
          article!.author.profileName !== this.authService.user()!.name;
        if (currentUserIsNotTheAuthor) {
          console.log('user: ' + this.authService.user());
          console.log('article: ' + article);
          this.router.navigateByUrl(`/`);
        }
      });
    });
  }

  ngOnInit() {
    this.state.update((state) => ({
      ...state,
      status: this.slug ? 'Editing' : 'Creating',
    }));

    if (this.slug) {
      this.article$.subscribe((article) =>
        this.state.update((state) => ({
          ...state,
          articleInProgress: article ?? undefined,
        }))
      );
      this.articleService.get(this.slug);
      return;
    }
  }

  updateArticleInProgress(article: ArticleInProgress) {
    this.state.update((state) => ({
      ...state,
      articleInProgress: article,
    }));
  }

  submit() {
    this.status() === 'Editing'
      ? this.articleService.edit(this.article() as EditArticleRequest)
      : this.articleService.create(this.article() as CreateArticleRequest);
  }
}
