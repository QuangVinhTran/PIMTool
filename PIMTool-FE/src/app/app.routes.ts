import { Routes } from '@angular/router';
import {AppComponent} from "./app.component";
import {NavigationBarComponent} from "./components/navigation-bar/navigation-bar.component";
import {HomeComponent} from "./pages/home/home.component";
import {ProjectListComponent} from "./components/home-components/content/project-list/project-list.component";
import {ContentComponent} from "./components/home-components/content/content.component";
import {LoginPageComponent} from "./pages/login-page/login-page.component";
import {authenticationGuard} from "./guards/authentication.guard";
import {NewProjectComponent} from "./components/home-components/content/new-project/new-project.component";
import {NotFoundPageComponent} from "./pages/not-found-page/not-found-page.component";

export const routes: Routes = [
  {path: "", component: HomeComponent, canActivateChild: [authenticationGuard], children: [
      {path: "", component: ProjectListComponent, title: "Project Information Management System"},
      {path: "new", component: NewProjectComponent, title: "New Project"}
    ]},
  {path: "login", component: LoginPageComponent, title: "Login to the system"},
  {path: "**", component: NotFoundPageComponent, title: "Not found"}
];
