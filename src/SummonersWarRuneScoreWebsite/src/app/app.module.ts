import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';


import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { RunesPageComponent } from './components/runes-page/runes-page.component';
import { RolesPageComponent } from './components/roles-page/roles-page.component';
import { NavBarStateService } from './services/nav-bar-state.service';
import { RunesGridComponent } from './components/runes-grid/runes-grid.component';

const appRoutes: Routes = [
  {
    path: 'Runes',
    component: RunesPageComponent
  },
  {
    path: 'Roles',
    component: RolesPageComponent
  },
  {
    path: '',
    component: HomeComponent
  },
  { path: '**', component: HomeComponent }
];


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavBarComponent,
    RunesPageComponent,
    RolesPageComponent,
    RunesGridComponent
  ],
  imports: [
    NgbModule.forRoot(),
    BrowserModule,
    RouterModule.forRoot(
      appRoutes
    ),
    NgxDatatableModule
  ],
  providers: [NavBarStateService],
  bootstrap: [AppComponent]
})
export class AppModule { }
