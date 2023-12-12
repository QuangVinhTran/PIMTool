import {Component, OnDestroy, OnInit} from '@angular/core';
import {CommonModule, DatePipe} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {GroupService} from "../services/group.service";
import {ProjectService} from "../services/project.service";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {Group} from "../models/group.model";
import {Project} from "../models/project";
import {catchError, of, Subscription} from "rxjs";


@Component({
  selector: 'app-edit-project',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
  templateUrl: './edit-project.component.html',
  styleUrl: './edit-project.component.css'
})
export class EditProjectComponent implements OnInit, OnDestroy{
//Boolean to alert any null fields
  showAlert = false;

  //Check member inputs
  membersInput?: string;
  nonExistentialVisas: string[];

  //model
  model: Project;
  groups?: Group[];

  //Subscription
  checkMemberVisaSubscription?: Subscription;
  editAProjectSubscription?: Subscription;
  paramMapSubscription?: Subscription;
  getAllGroupsSubscription?: Subscription;
  getAProjectSubscription?: Subscription;


  constructor(private groupService: GroupService, private projectService: ProjectService,
              private router: Router, private route: ActivatedRoute, private datePipe: DatePipe) {
    this.nonExistentialVisas = [];
    this.model = {};
  }


  onFormSubmit() {
    if(  this.model.name?.trim() === '' || this.model.name?.trim() === undefined
      || this.model.customer?.trim() === '' || this.model.customer?.trim() === undefined
      || this.model.groupId === null || this.model.groupId === undefined
      || this.model.startDate === null || this.model.startDate === undefined) {
      this.showAlert = true;
      return;
    }

    if(this.membersInput?.trim() !== "" && this.membersInput !== undefined)
        this.model.employeeVisas = this.membersInput!.trim().replace(/\s/g, '').split(",");
    else {
        this.membersInput = "";
        this.model.employeeVisas = [];
    }
      this.checkMemberVisaSubscription = this.projectService.checkMemberVisa(this.membersInput!.trim().replace(/\s/g, ''))
        .pipe(
          catchError((error) => this.handleError(error))
        )
        .subscribe({
          next: (response) => {
            this.nonExistentialVisas = response;
            if(this.nonExistentialVisas.length > 0){
                return;
            }

              this.editAProjectSubscription = this.projectService.editAProject(this.model)
                .pipe(
                  catchError((error) => this.handleError(error))
                )
                .subscribe({
                  next:() => {
                    this.router.navigateByUrl('/project/list')
                  },
                  error(err) {
                    console.log(err);
                  }
                });

          },
          error(err) {
            console.log(err);
          }
        });

  }

  closeAlert() {
    this.showAlert = false;
  }



  ngOnInit(): void {
    this.paramMapSubscription = this.route.paramMap.subscribe({
        next: (params) => {
          this.model.id =  parseInt(params.get('id')!.toString(),10);
        }
    })

    this.getAllGroupsSubscription = this.groupService.getAllGroups()
      .pipe(
        catchError((error) => this.handleError(error))
      )
      .subscribe({
        next: (response) => {
          this.groups = response;
        },
        error(err){},
        complete(){}
      });

    this.getAProjectSubscription = this.projectService.getAProject(this.model.id!)
      .pipe(
        catchError((error) => this.handleError(error))
      )
      .subscribe({
            next:(response) => {
              this.model = response;
              this.membersInput = this.model.employeeVisas?.toString();
              if(this.model.startDate !== null && this.model.startDate !== undefined)
                this.model.startDate = this.datePipe.transform(this.model.startDate, 'yyyy-MM-dd');
              if(this.model.endDate !== null && this.model.endDate !== undefined)
                this.model.endDate = this.datePipe.transform(this.model.endDate, 'yyyy-MM-dd');
            }
        });

  }

  handleError(error: any) {
    // Navigate to the error page using the Router
    this.router.navigate(['/error']);
    return of();

  }

    ngOnDestroy(): void {
      this.checkMemberVisaSubscription?.unsubscribe();
      this.editAProjectSubscription?.unsubscribe();
      this.paramMapSubscription?.unsubscribe();
      this.getAllGroupsSubscription?.unsubscribe();
      this.getAProjectSubscription?.unsubscribe();
    }
}
