import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListProjectComponent } from './component/list-project/list-project.component';
import { ProjectDetailComponent } from './component/project-detail/project-detail.component';
import { ServerErrorComponent } from './component/server-error/server-error.component';

const routes: Routes = [
  { path: '', component: ListProjectComponent },
  { path: 'list', component: ListProjectComponent },
  { path: 'project/:projectNumber', component: ProjectDetailComponent },
  { path: 'project', component: ProjectDetailComponent },
  { path: 'error', component: ServerErrorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
