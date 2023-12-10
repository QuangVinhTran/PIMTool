import { ProjectService } from '../services/project.service';
import { Group } from '../../group/models/group.model';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupService } from '../../group/services/group.service';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CreateProjectRequest } from '../models/create-project.model';
import { Employee } from '../../employee/models/employee.model';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { CalendarModule } from 'primeng/calendar';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-create-project',
  standalone: true,
  imports: [
    CommonModule,
    DropdownModule,
    FormsModule,
    ReactiveFormsModule,
    MultiSelectModule,
    NgbModule,
    CalendarModule,
    NgxSpinnerModule,
  ],
  templateUrl: './add-edit-project.component.html',
  styleUrl: './add-edit-project.component.scss',
})
export class CreateProjectComponent implements OnInit {
  formCreateProject!: FormGroup;
  groupList!: Group[];
  statusList: any[] = [
    { name: 'New', value: 'NEW' },
    { name: 'Planned', value: 'PLA' },
    { name: 'In progress', value: 'INP' },
    { name: 'Finished', value: 'FIN' },
  ];
  employeeList!: Employee[];
  isAddMode!: boolean;
  id!: number;
  isFillForm: boolean = true;
  isDateRangeValid: boolean = true;
  selectedCustomers!: string[];

  constructor(
    private groupService: GroupService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {
    this.formCreateProject = this.fb.group({
      projectNumber: ['', Validators.required],
      name: ['', Validators.required],
      customer: ['', Validators.required],
      groupId: ['', Validators.required],
      members: [[]],
      status: ['NEW', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      version: [''],
    });
  }

  get projectNumber() {
    return this.formCreateProject.controls['projectNumber'];
  }
  get name() {
    return this.formCreateProject.controls['name'];
  }
  get customer() {
    return this.formCreateProject.controls['customer'];
  }
  get groupId() {
    return this.formCreateProject.controls['groupId'];
  }
  get members() {
    return this.formCreateProject.controls['members'];
  }
  get startDate() {
    return this.formCreateProject.controls['startDate'];
  }

  async ngOnInit() {
    this.spinner.show();

    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    //   this.spinner.hide();
    // }, 5000);
    this.id = parseInt(this.route.snapshot.params['id']);
    this.isAddMode = !this.id;

    await this.groupService.getAllGroup().subscribe((data) => {
      this.groupList = data;
    });
    if (!this.isAddMode) {
      this.projectService.GetByProjectNumber(this.id).subscribe((project) => {
        this.formCreateProject.controls['projectNumber'].setValue(
          project.projectNumber
        );
        this.formCreateProject.controls['name'].setValue(project.name);
        this.formCreateProject.controls['customer'].setValue(project.customer);
        this.formCreateProject.controls['status'].setValue(project.status);
        this.formCreateProject.controls['startDate'].setValue(
          this.convertDateFormat(project.startDate.split('T')[0])
        );
        this.formCreateProject.controls['endDate'].setValue(
          this.convertDateFormat(project?.endDate.split('T')[0])
        );
        this.formCreateProject.controls['groupId'].setValue(project.groupId);
        this.formCreateProject.controls['members'].setValue(project.members);
        this.formCreateProject.controls['version'].setValue(project.version);

        const selectedGroup = this.groupList.find(
          (group) => group.id === project.groupId
        );
        if (selectedGroup) {
          this.employeeList = selectedGroup.employees;
        }
      });
    }
  }

  changeGroup(selectedGroupId: number) {
    const selectedGroup = this.groupList.find(
      (group) => group.id === selectedGroupId
    );

    if (selectedGroup) {
      this.formCreateProject.patchValue({
        members: [],
      });
      this.employeeList = selectedGroup.employees;
    }
  }

  validatorsDateRange(startDate: string, endDate: string): boolean {
    const startDateObj = new Date(startDate);
    if (!endDate) return true;
    const endDateObj = new Date(endDate);

    return startDateObj <= endDateObj;
  }

  cancel() {
    this.router.navigateByUrl('/home');
  }

  convertDateFormat(dateString: string) {
    if (!dateString) {
      return null;
    }

    const parts = dateString.split('-');
    if (parts.length !== 3) {
      // Invalid date format
      return null;
    }

    const [year, month, day] = parts;
    const formattedDate = `${day}.${month}.${year}`;

    return formattedDate;
  }

  onFormSubmit() {
    if (!this.formCreateProject.valid) {
      this.isFillForm = false;
      return;
    }
    const postData = { ...this.formCreateProject.value };
    if (!this.validatorsDateRange(postData.startDate, postData.endDate)) {
      this.isDateRangeValid = false;
      return;
    }
    if (this.isAddMode) {
      this.projectService
        .CreateProject(postData as CreateProjectRequest)
        .subscribe({
          next: (response: any) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Create project successfully',
            });
            this.router.navigateByUrl('/home');
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: err.error,
            });
          },
        });
    } else {
      this.projectService
        .UpdateProject(
          this.formCreateProject.value.projectNumber,
          postData as CreateProjectRequest
        )
        .subscribe({
          next: (response: any) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Update project successfully',
            });
            this.router.navigateByUrl('/home');
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: err.error,
            });
          },
        });
    }
  }
}
