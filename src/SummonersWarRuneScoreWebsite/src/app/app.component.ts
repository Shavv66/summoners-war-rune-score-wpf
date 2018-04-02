import { Component, Renderer2 } from '@angular/core';
import { NavBarStateService } from './services/nav-bar-state.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'Summoners War Rune Score';

  constructor(private renderer: Renderer2, private navBarStateService: NavBarStateService) {
    this.navBarStateService.renderer = this.renderer;
    //Sets Body class for theme
    this.renderer.addClass(document.body, "fixed-nav");
    this.renderer.addClass(document.body, "bg-dark");


    // Collapse side nav bar by default
    this.navBarStateService.SetNavBarCollapsed(true);
  }
}
