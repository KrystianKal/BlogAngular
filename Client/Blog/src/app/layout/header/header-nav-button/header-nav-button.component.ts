import { NgClass } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-header-nav-button',
  standalone: true,
  imports: [NgClass, MatIcon, RouterLink],
  template: `
    <a
      [ngClass]="
        this.router.url === link()
          ? 'text-textPrimary font-medium'
          : 'text-textSecondaryBright'
      "
      routerLink="{{ link() }}"
      class="flex flex-row "
    >
      <mat-icon>{{ icon() }}</mat-icon>
      <ng-content />
    </a>
  `,
})
export class HeaderNavButtonComponent {
  link = input.required<string>();
  icon = input.required<string>();

  public router = inject(Router);
}
