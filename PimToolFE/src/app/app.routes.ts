import { Routes } from '@angular/router';
import {CreateProjectComponent} from "./core/features/project/create-project/create-project.component";
import {ProjectListComponent} from "./core/features/project/project-list/project-list.component";
import {EditProjectComponent} from "./core/features/project/edit-project/edit-project.component";
import {ErrorComponent} from "./core/components/error/error.component";

export const routes: Routes = [
  {
    path: "project/create",
    component: CreateProjectComponent
  },
  {
    path: "project/list",
    component: ProjectListComponent
  },
  {
    path: "project/list/:page",
    component: ProjectListComponent
  },
  {
    path: "project/edit/:id",
    component: EditProjectComponent
  },
  {
    path: "error",
    component: ErrorComponent
  }
];
