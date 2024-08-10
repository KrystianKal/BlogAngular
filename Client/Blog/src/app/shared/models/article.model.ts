import { Author } from './author.model';

export type Article = {
  slug: string;
  title: string;
  description: string;
  body: string;
  tagList: string[];
  createdAt: string;
  updatedAt: string;
  favorited?: boolean;
  favoritesCount: number;
  author: Author;
};

export type Articles = {
  articles: Article[];
  articlesCount: number;
};
