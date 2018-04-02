import { Component, OnInit, Renderer2 } from '@angular/core';
import { NavBarStateService } from '../../services/nav-bar-state.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  constructor(private navBarStateService: NavBarStateService, private renderer: Renderer2) { 
    this.navBarStateService.renderer = this.renderer;
  }

  ngOnInit() {
  }

  sideNavToggleClicked() {
    this.navBarStateService.ToggleNavBarCollapsed();
  }

}
