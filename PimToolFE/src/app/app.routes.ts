import { Routes } from '@angular/router';
import {CreateProjectComponent} from "./core/features/project/create-project/create-project.component";
import {ProjectListComponent} from "./core/features/project/project-list/project-list.component";

export const routes: Routes = [
  {
    path: "project/create",
    component: CreateProjectComponent
  },
  {
    path: "project/list",
    component: ProjectListComponent
  }
];
