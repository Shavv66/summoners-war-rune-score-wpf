import { Injectable, Renderer2 } from '@angular/core';

@Injectable()
export class NavBarStateService {

  public renderer: Renderer2;

  private NavBarCollapsed: boolean = false;

  constructor() { }

  public ToggleNavBarCollapsed() {
    this.SetNavBarCollapsed(!this.NavBarCollapsed);
  }

  public SetNavBarCollapsed(collapse: boolean) {
    if (collapse != this.NavBarCollapsed) {
      if (collapse) {
        this.renderer.addClass(document.body, "sidenav-toggled");
      } else {
        this.renderer.removeClass(document.body, "sidenav-toggled");
      }
      this.NavBarCollapsed = collapse;
    }
  }
}
