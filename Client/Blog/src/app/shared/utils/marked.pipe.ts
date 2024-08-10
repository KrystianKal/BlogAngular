import { inject, Pipe, PipeTransform, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { marked } from 'marked';

@Pipe({
  standalone: true,
  name: 'Marked',
})
export class MarkedPipe implements PipeTransform {
  private sanitizer = inject(DomSanitizer);
  transform(value: string) {
    // return this.sanitizer.sanitize(SecurityContext.HTML, marked(value));
    return this.sanitizer.sanitize(SecurityContext.HTML, marked.parse(value));
  }
}
