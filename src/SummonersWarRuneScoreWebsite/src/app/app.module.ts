import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RunesPageComponent } from './runes-page/runes-page.component';
import { RolesPageComponent } from './roles-page/roles-page.component';

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
    RolesPageComponent
  ],
  imports: [
    NgbModule,
    NgbModule.forRoot(),
    BrowserModule,
    RouterModule.forRoot(
      appRoutes
    ),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
