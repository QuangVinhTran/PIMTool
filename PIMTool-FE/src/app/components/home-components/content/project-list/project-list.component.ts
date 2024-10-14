import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {MatSortModule, Sort} from "@angular/material/sort";
import {Project} from "../../../../model/project";
import {MatCheckboxChange, MatCheckboxModule} from "@angular/material/checkbox";
import {MatIcon, MatIconModule} from "@angular/material/icon";
import {FormsModule} from "@angular/forms";
import {SelectedItem} from "../../../../model/selected-item";
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatInput, MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {LocalStorageService} from "../../../../service/local-storage.service";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {ProjectService} from "../../../../service/project.service";
import {Response} from "../../../../model/response";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {Router} from "@angular/router";

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatSortModule, MatCheckboxModule, FormsModule, MatAutocompleteModule,
          MatInputModule, MatFormFieldModule, HttpClientModule],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.css',
  providers: [LocalStorageService, HttpClient, ProjectService]
})
export class ProjectListComponent{
  checkedItem: SelectedItem = {numberOfItemSelected: 0, listOfItemSelected: []};
  projects : Project[] = [];
  // projects: Project[] = []
  sortedData: Project[] = [];
  data: Response = {message: "", data: []};
  constructor(private localStorageService: LocalStorageService,
              private projectService: ProjectService,
              private router: Router) {
    this.projectService.getProjects(this.localStorageService.getDataFromLocal("token")).subscribe(data => {
      this.projects = data.data
      this.sortedData = this.projects;
    }, error1 => {
      localStorageService.removeDataFromLocal("token");
      router.navigate(['/login'])
    });
  }

  onKey(event: any) {
    if (event.target.value !== "") {
      console.log(this.projects);
      this.sortedData = this.projects.slice().filter((data) => data.projectNumber.toString().includes(event.target.value) ||
        data.name.toLowerCase().includes(event.target.value.toLowerCase()));
    } else {
      this.sortedData = this.projects;
    }
  }

  onChangeCheckBox(event: MatCheckboxChange, id: number) {
    if (event.checked) {
      this.checkedItem.numberOfItemSelected += 1;
      this.checkedItem.listOfItemSelected.push(id);
    }
    else {
      this.checkedItem.numberOfItemSelected -= 1;
      const index: number = this.checkedItem.listOfItemSelected.indexOf(id);
      this.checkedItem.listOfItemSelected.splice(index, 1);
    }
  }

  sortData(sort: Sort) {
    const data = this.sortedData;
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'projectNumber':
          return compare(a.projectNumber, b.projectNumber, isAsc);
        case 'name':
          return compare(a.name, b.name, isAsc);
        case 'customer':
          return compare(a.customer, b.customer, isAsc);
        case 'startDate':
          return compare(a.startDate, b.startDate, isAsc);
        case 'endDate':
          return compare(a.endDate, b.endDate, isAsc);
        default:
          return 0;
      }
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
