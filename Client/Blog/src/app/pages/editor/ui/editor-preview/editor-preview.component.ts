import { Component, input } from '@angular/core';
import { ArticleInProgress } from '../../editor.component';
import { MarkedPipe } from '../../../../shared/utils/marked.pipe';

@Component({
  selector: 'app-editor-preview',
  standalone: true,
  imports: [MarkedPipe],
  templateUrl: './editor-preview.component.html',
})
export class EditorPreviewComponent {
  article = input.required<ArticleInProgress>();
}
