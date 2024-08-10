import { NgClass } from '@angular/common';
import { Component, input } from '@angular/core';

type Size = 'Small' | 'Large';
@Component({
  selector: 'app-avatar',
  standalone: true,
  imports: [NgClass],
  template: `<div>
    <img
      class="relative object-cover object-center rounded-lg m-1"
      [ngClass]="size() === 'Small' ? 'h-12 w-12' : 'h-60 w-60'"
      src="{{ imageUrl() ?? '/person.png' }}"
    />
  </div>`,
})
export class AvatarComponent {
  imageUrl = input<string | null>(null);
  size = input<Size>('Small');
}
