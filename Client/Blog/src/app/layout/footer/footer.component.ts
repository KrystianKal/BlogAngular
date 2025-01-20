import { Component } from '@angular/core';

@Component({
    selector: 'app-footer',
    imports: [],
    template: `
    <div
      class="bg-slate-100 w-full flex items-center justify-center font-light text-xs  text-slate-400 p-2 mt-2"
    >
      <p style="margin: 0px">
        © 2024. An interactive learning project made in Angular.
      </p>
    </div>
  `
})
export class FooterComponent {}
