import { Component, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  constructor(private renderer: Renderer2) {
    //Sets Body class for theme
    this.renderer.addClass(document.body, "fixed-nav");
    this.renderer.addClass(document.body, "bg-dark");
  }
}
