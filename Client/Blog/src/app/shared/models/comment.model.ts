import { Author } from './author.model';

export type Comment = {
  id: string;
  createdAt: string;
  updatedAt: string | null;
  body: string;
  author: Author;
};
