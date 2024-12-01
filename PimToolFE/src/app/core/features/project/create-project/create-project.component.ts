import {Component, OnDestroy, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule} from "@angular/forms";
import {Project} from "../models/project";
import {Router, RouterLink} from "@angular/router";
import {GroupService} from "../services/group.service";
import {Group} from "../models/group.model";
import {ProjectService} from "../services/project.service";
import {catchError, forkJoin, of, Subscription} from "rxjs";

@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.css'
})
export class CreateProjectComponent implements OnInit, OnDestroy{
  //Boolean to alert any null fields
  showAlert = false;

  //Check member inputs
  membersInput?: string;
  nonExistentialVisas: string[];

  //model
  model: Project;
  groups?: Group[];

  //Check existed project number
  duplicatedNumber: Project | null

  //status
  status: boolean = true;

  //Subscription
  projectValidationSubscription?: Subscription;
  projectSubscription? : Subscription;
  groupSubscription? : Subscription;

  constructor(private groupService: GroupService, private projectService: ProjectService,
              private router: Router) {
    this.model = {
      status : "NEW"
    }

    this.duplicatedNumber = null;
    this.nonExistentialVisas = [];
  }


  onFormSubmit() {
    if(  this.model.projectNumber! === null || this.model.projectNumber! === undefined
      || this.model.name?.trim() === '' || this.model.name?.trim() === undefined
      || this.model.customer?.trim() === '' || this.model.customer?.trim() === undefined
      || this.model.groupId === null || this.model.groupId === undefined
      || this.model.startDate === null || this.model.startDate === undefined) {
      this.showAlert = true;
      return;
    }

      if(this.membersInput !== null && this.membersInput !== undefined) {
          //remove space and split the string into array of string
          //assign the members to a project
          //create a project
          this.model.employeeVisas = this.membersInput!.trim().replace(/\s/g, '').split(",");
          const checkProjectNumberObservable = this.projectService.checkProjectNumber(this.model.projectNumber!);
          const checkMemberVisaObservable = this.projectService.checkMemberVisa(this.membersInput!.trim().replace(/\s/g, ''));

           this.projectValidationSubscription = forkJoin([checkProjectNumberObservable, checkMemberVisaObservable])
             .pipe(
               catchError((error) => this.handleError(error))
             )
             .subscribe({
                  next: ([projectNumberResponse, memberVisaResponse]) => {
                      this.duplicatedNumber = projectNumberResponse;
                      this.status = this.duplicatedNumber === null;

                      this.nonExistentialVisas = memberVisaResponse;
                      this.status = this.nonExistentialVisas.length === 0;

                      // If both checks are successful, create the project
                      if (this.status) {
                          this.projectSubscription =  this.projectService.createAProject(this.model)
                            .pipe(
                              catchError((error) => this.handleError(error))
                            )
                            .subscribe({
                                  next:(response) => {
                                      if(this.status)
                                          this.router.navigateByUrl('/project/list')
                                  },
                                  error(err) {
                                      return;
                                  }
                              });
                      }
                  },
                  error: (err) => {
                      console.log(err);
                  }
              });

      } else {
          this.membersInput = "";
          this.model.employeeVisas = [];
          const checkProjectNumberObservable = this.projectService.checkProjectNumber(this.model.projectNumber!);
          this.projectValidationSubscription = checkProjectNumberObservable
            .pipe(
              catchError((error) => this.handleError(error))
            )
            .subscribe({
              next: (response) => {
                  this.duplicatedNumber = response;
                  if (this.duplicatedNumber !== null) {
                      this.status = false;
                  }

                  // If project number check is successful, create the project
                  if (this.status) {
                      this.projectSubscription = this.projectService.createAProject(this.model)
                        .pipe(
                          catchError((error) => this.handleError(error))
                        )
                        .subscribe({
                              next:(response) => {
                                  if(this.status)
                                      this.router.navigateByUrl('/project/list')
                              },
                              error(err) {
                                  return;
                              }
                          });
                  }
              },
              error: (err) => {
                  console.log(err);
              }
          });
      }


  }

  closeAlert() {
    this.showAlert = false;
  }


  ngOnInit(): void {
    this.groupSubscription = this.groupService.getAllGroups()
      .pipe(
        catchError((error) => this.handleError(error))
      )
      .subscribe({
        next: (response) => {
          this.groups = response
    },
        error(err){},
        complete(){}
      });

  }

  handleError(error: any) {

    // Navigate to the error page using the Router
    this.router.navigate(['/error']);
    return of();

  }

    ngOnDestroy(): void {
      this.projectValidationSubscription?.unsubscribe();
      this.projectSubscription?.unsubscribe();
      this.groupSubscription?.unsubscribe();
    }

}
