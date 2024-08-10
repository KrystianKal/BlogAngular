import { AsyncPipe, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, output, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BehaviorSubject } from 'rxjs';

interface TagsResponse {
  tags: string[];
}
interface TagsState {
  tags: string[] | undefined;
  selectedTag: string | null;
}

@Component({
  selector: 'app-tags',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatChipsModule, AsyncPipe, NgIf],
  templateUrl: './tags.component.html',
})
export class TagsComponent {
  private http = inject(HttpClient);

  tagChanged = output<string | null>();

  private tags$ = this.http.get<TagsResponse>('api/tags');

  private state = signal<TagsState>({
    tags: undefined,
    selectedTag: null,
  });

  tags = computed(() => this.state().tags);
  selectedTag = computed(() => this.state().selectedTag);

  constructor() {
    this.tags$.pipe(takeUntilDestroyed()).subscribe((tags) =>
      this.state.update((state) => ({
        ...state,
        tags: tags.tags,
      }))
    );
  }

  selectionChange(tag: string) {
    const selectedTag = this.state().selectedTag === tag ? null : tag;
    this.tagChanged.emit(selectedTag);
    this.state.update((state) => ({
      ...state,
      selectedTag: selectedTag,
    }));
  }
}
