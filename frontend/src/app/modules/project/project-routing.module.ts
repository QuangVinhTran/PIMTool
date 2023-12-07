import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {ProjectListComponent} from "./project-list/project-list.component";
import {CreateProjectComponent} from "./create-project/create-project.component";

const routes:Routes = [
  {
    path: '',
    redirectTo: 'project-list',
    pathMatch: 'full'
  },
  {
    path: 'project-list',
    component: ProjectListComponent
  },
  {
    path: 'create',
    component: CreateProjectComponent
  },
  {
    path: 'update/:id',
    component: CreateProjectComponent
  }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
})
export class ProjectRoutingModule { }
