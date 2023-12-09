import { Component, ViewEncapsulation } from '@angular/core';
import { Project } from 'src/shared/models/project';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class LayoutComponent {
  project? : Project;
  createProject() {
    this.project = new Project();
  }
  
}
