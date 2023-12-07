import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule} from "@angular/forms";
import {CreateProjectModel} from "../models/create-project.model";
import {RouterLink} from "@angular/router";
import {GroupService} from "../services/group.service";
import {Group} from "../models/group.model";
import {ProjectService} from "../services/project.service";

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.css'
})
export class CreateProjectComponent implements OnInit{
  showAlert = false;
  membersInput: string;
  model: CreateProjectModel;
  groups?: Group[];
  constructor(private groupService: GroupService, private projectService: ProjectService) {
    this.model = {
      status : 0
    }
    this.membersInput = ""
  }



  onFormSubmit() {
    if(  this.model.projectNumber?.trim() === '' || this.model.projectNumber?.trim() === undefined
      || this.model.name?.trim() === '' || this.model.name?.trim() === undefined
      || this.model.customer?.trim() === '' || this.model.customer?.trim() === undefined
      || this.model.groupId === null || this.model.groupId === undefined
      || this.model.startDate === null || this.model.startDate === undefined)
      this.showAlert = true;
    else {
      this.showAlert = false;
      this.model.employeeVisas = this.membersInput.split(",");
      this.projectService.createAProject(this.model)
        .subscribe({
          next(x) {
            console.log('got value ' + x);
          },
          error(err) {
            console.log(err);
          },
          complete() {
            console.log('done');
          },
        })
    }

  }

  closeAlert() {
    this.showAlert = false;
  }

  ngOnInit(): void {
    this.groupService.getAllGroups()
      .subscribe({
        next: (response) => {
          this.groups = response
    }
      });

  }
}
