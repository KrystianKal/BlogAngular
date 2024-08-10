import {
  Component,
  computed,
  inject,
  input,
  model,
  output,
  signal,
} from '@angular/core';
import { ArticleInProgress, EditorStatus } from '../../editor.component';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { getErrorMessage } from '../../../../shared/utils/form-validation.utils';

@Component({
  selector: 'app-editor-form',
  standalone: true,
  imports: [
    MatChipsModule,
    FormsModule,
    MatIconModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    NgIf,
    MatInputModule,
  ],
  templateUrl: './editor-form.component.html',
})
export class EditorFormComponent {
  http = inject(HttpClient);
  getError = getErrorMessage;

  article = input.required<ArticleInProgress>();
  status = input.required<EditorStatus>();
  articleChange = output<ArticleInProgress>();
  submit = output();

  readonly currentTag = model('');
  readonly allTags = signal<string[]>([]);
  readonly filteredTags = computed(() => {
    const currentTag = this.currentTag().toLowerCase();
    return currentTag
      ? this.allTags().filter((tag) => tag.toLowerCase().includes(currentTag))
      : this.allTags().slice();
  });

  constructor() {
    this.http
      .get<{ tags: string[] }>('api/tags')
      .pipe(takeUntilDestroyed())
      .subscribe((res) => this.allTags.set(res.tags));
  }

  removeTag(tag: string) {
    this.article().tagList = this.article().tagList.filter(
      (tagName) => tagName !== tag
    );
    this.emitChange();
  }

  addTag(event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if (value && !this.article().tagList.includes(value)) {
      this.article().tagList = [...this.article().tagList, value];
      this.emitChange();
    }
    event.chipInput!.clear();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (!this.article().tagList.includes(event.option.viewValue)) {
      this.article().tagList = [
        ...this.article().tagList,
        event.option.viewValue,
      ];
    }
    this.currentTag.set('');
    event.option.deselect();
  }
  onSubmit() {
    this.submit.emit();
  }
  emitChange() {
    this.articleChange.emit(this.article());
  }
}
