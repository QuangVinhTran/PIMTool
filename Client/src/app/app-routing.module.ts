import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { HomeComponent } from './features/public/home/home.component';
import { ProjectListComponent } from './features/project/project-list/project-list.component';
import { CreateProjectComponent } from './features/project/add-edit-project/add-edit-project.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  // {
  //   path: '',
  //   redirectTo: 'home',
  //   pathMatch: 'full',
  // },
  {
    path: 'home',
    component: HomeComponent,
    children: [
      {
        path: '',
        redirectTo: 'project',
        pathMatch: 'full',
      },
      {
        path: 'project',
        children: [
          {
            path: '',
            redirectTo: 'list',
            pathMatch: 'full',
          },
          {
            path: 'list',
            component: ProjectListComponent,
          },
          {
            path: 'new',
            component: CreateProjectComponent,
          },
          {
            path: 'update/:id',
            component: CreateProjectComponent,
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
