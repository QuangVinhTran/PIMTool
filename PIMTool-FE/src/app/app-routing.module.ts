import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateProjectComponent } from './components/project/create-project/create-project.component';
import { ListProjectComponent } from './components/project/list-project/list-project.component';
import { EditProjectComponent } from './components/project/edit-project/edit-project.component';
const routes: Routes = [
  { path: '', component: ListProjectComponent },
  { path: 'create/project', component: CreateProjectComponent },
  { path: 'edit/project/:id', component: EditProjectComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
