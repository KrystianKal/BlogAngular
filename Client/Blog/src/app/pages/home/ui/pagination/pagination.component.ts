import { Component, effect, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './pagination.component.html',
})
export class PaginationComponent {
  itemCount = input.required<number>();
  pageSize = input.required<number>();
  page = input.required<number>();
  pageChanged = output<number>();

  totalPages: number = 0;

  constructor() {
    effect(() => {
      this.totalPages = Math.ceil(this.itemCount() / this.pageSize());
    });
  }
}
