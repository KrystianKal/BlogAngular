import { Component, inject, input, Input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../../shared/auth.service';
import { Article } from '../../../../shared/models/article.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-article-socials',
  standalone: true,
  imports: [MatIconModule, NgClass],
  templateUrl: './article-socials.component.html',
})
export class ArticleSocialsComponent {
  article = input.required<Article>();
  articleFavorited = output();
}
