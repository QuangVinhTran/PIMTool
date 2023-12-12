import {Component, OnDestroy, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {ProjectService} from "../services/project.service";
import {Project} from "../models/project";
import {ProjectParameters} from "../models/project-parameter.model";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {catchError, of, Subscription} from "rxjs";
import {FormsModule} from "@angular/forms";
import {FilterService} from "../services/filter.service";


@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.css'
})
export class ProjectListComponent implements OnInit, OnDestroy{

  projects?: Project[];
  model: ProjectParameters;

  currentPage: number = 1;
  totalPageIndex: number[] = [];
  selectedDeletingRows: number[] = [];

  //Subscription
  paramMapSubscription?: Subscription;
  getAllProjectsSubscription?: Subscription;
  deleteAProjectSubscription?: Subscription;
  filterProjectSubscription?: Subscription;

  constructor(private projectService: ProjectService,private route: ActivatedRoute,
              private router:Router, private filterService: FilterService) {
    this.model = {
      pagingParameters: {
        pageNumber: 1,
        pageSize: 5
      },
      status: ""
    };
  }

  ngOnInit(): void {

    this.paramMapSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        if(params.get('page') !== null && params.get('page') !== undefined) {
          this.model.pagingParameters.pageNumber =  parseInt(params.get('page')!,10);
          this.currentPage = this.model.pagingParameters.pageNumber;
        }
        this.getAllProjectsSubscription = this.projectService.getAllProjects(this.model)
          .pipe(
            catchError((error) => this.handleError(error))
          )
          .subscribe({
            next: (response) => {
              this.projects = response;
              this.totalPageIndex = new Array(this.projects[0].totalPage).fill(0)
                .map((n, index) => index + 1);
            }
          });
      }
    });

    this.model.filterParameters = this.filterService.getFilterValue();
    this.model.status = this.filterService.getfilterStatus();



  }
  onDelete(id: number) {
    if (confirm("Are you want to delete this project?")) {
      this.deleteAProjectSubscription = this.projectService.deleteAProject(id)
        .pipe(
          catchError((error) => this.handleError(error))
        )
        .subscribe({
          next:(response) => {
            location.reload();
      },
          error(err) {
            console.log(err)
      }
        })
    }
  }
  onFormSubmit(){
    this.selectedDeletingRows = [];
    this.filterProjectSubscription = this.projectService.getAllProjects(this.model)
      .pipe(
        catchError((error) => this.handleError(error))
      )
        .subscribe({
          next:(response) => {
            this.projects = response;
            this.totalPageIndex = new Array(this.projects[0].totalPage).fill(0)
                .map((n, index) => index + 1);
          }
        })
  }

  onReset() {
    this.model.filterParameters = undefined;
    this.model.status = "";
  }

  addDeletingRow(event: any,id: number) {
    if (event.target.checked) {
        this.selectedDeletingRows.push(id);
    } else {
      this.selectedDeletingRows = this.selectedDeletingRows.filter(number => number !== id);
    }
  }

  onDeleteProjects() {
    if (confirm(`Are you sure want to delete ${this.selectedDeletingRows.length} projects?`)) {
      this.projectService.deleteMultipleProjects(this.selectedDeletingRows)
        .pipe(
          catchError((error) => this.handleError(error))
        )
        .subscribe({
          next: (response) => {
            location.reload();
          },
          error(err) {
            console.log(err)
          }
        })
    }
  }

  handleError(error: any) {
    // You can log the error to the console or perform other error handling tasks.
    console.error('Error occurred:', error);

    // Navigate to the error page using the Router
    this.router.navigate(['/error']);
    return of();

  }

  ngOnDestroy(): void {
    this.paramMapSubscription?.unsubscribe();
    this.getAllProjectsSubscription?.unsubscribe();
    this.deleteAProjectSubscription?.unsubscribe();
    this.filterProjectSubscription?.unsubscribe();

    if(this.model.filterParameters !== undefined)
      this.filterService.setFilterValue(this.model.filterParameters);
    if(this.model.status !== undefined)
      this.filterService.setfilterStatus(this.model.status);
  }

}
