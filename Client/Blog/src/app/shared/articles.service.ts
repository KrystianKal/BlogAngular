import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Article } from './models/article.model';

export type ArticlesResponse = {
  articles: Article[];
  articleCount: number;
};

export interface ArticleParams {
  limit?: number;
  page?: number;
  tag?: string | null;
  author?: string | null;
  favorited?: string | null;
}

@Injectable({
  providedIn: 'root',
})
export class ArticlesService {
  http = inject(HttpClient);

  list(params?: ArticleParams) {
    let limit = params?.limit ?? 10;
    let offset = limit * (params?.page ?? 0);

    return this.http.get<ArticlesResponse>('api/articles', {
      params: {
        limit: limit,
        offset: offset,
        tag: params?.tag ?? '',
        author: params?.author ?? '',
        favorited: params?.favorited ?? '',
      },
    });
  }
  feed(params?: Pick<ArticleParams, 'page' | 'limit'>) {
    let limit = params?.limit ?? 10;
    let offset = limit * (params?.page ?? 0);

    return this.http.get<ArticlesResponse>('api/articles/feed', {
      params: {
        limit: limit,
        offset: offset,
      },
    });
  }
  constructor() {}
}
