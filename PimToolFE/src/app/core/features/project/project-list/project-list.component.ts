import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {ProjectService} from "../services/project.service";
import {Project} from "../models/project.model";
import {ProjectParameters} from "../models/project-parameter.model";

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.css'
})
export class ProjectListComponent implements OnInit{

  projects?: Project[];
  model: ProjectParameters;
  status: string[]

  constructor(private projectService: ProjectService) {
    this.model = {
      pagingParameters: {
        pageNumber: 1,
        pageSize: 5
      }
    };
    this.status = ['New', 'Planned', 'In progress', 'Finished']
  }

  ngOnInit(): void {
    this.projectService.getAllProjects(this.model)
      .subscribe({
        next: (response) => {
          this.projects = response
        }
      });
  }
}
