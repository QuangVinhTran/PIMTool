import { Component, Input, OnInit } from '@angular/core';
import {
  FormControl,
  ReactiveFormsModule,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { faSpinner } from '@fortawesome/free-solid-svg-icons';
import { ProjectService } from 'src/shared/services/project.service';
import { ProjectCreate } from 'src/shared/models/projectCreate';
import { ResponseDto } from 'src/shared/models/responseDto';
import { GroupService } from 'src/shared/services/group.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Employee } from 'src/shared/models/employee';
import { EmployeeService } from 'src/shared/services/employee.service';
import { ToastService } from 'src/shared/services/toast.service';
import { inRange } from 'src/shared/validators/inRange';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { Project } from 'src/shared/models/project';
import { EmployeeDropdown } from 'src/shared/models/employeeDropdown';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { of, catchError } from 'rxjs';

@Component({
  standalone: true,
  selector: 'app-project-new',
  templateUrl: './project-new.component.html',
  styleUrls: ['./project-new.component.css'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule,
    FontAwesomeModule,
    TranslateModule,
  ],
})
export class ProjectNewComponent implements OnInit {
  faSpinner = faSpinner;
  listGroupId: Number[] = this.groupService.getAllId();
  isUpdate = true;
  listEmployeeAll: Employee[] = [];
  validForm: boolean = true;
  formMessage: string = '';
  isExistProjectNumber: boolean = false;
  updateId = '';
  project = new Project();
  isChecking = false;
  isProcessing = false;
  messageLength = '';
  // Test
  dropdownList: EmployeeDropdown[] = [];
  selectedItems: EmployeeDropdown[] = [];
  dropdownSettings: IDropdownSettings = {};
  constructor(
    private formBuilder: FormBuilder,
    private projectService: ProjectService,
    private groupService: GroupService,
    private employeeService: EmployeeService,
    private toastService: ToastService,
    private route: Router,
    private translateService: TranslateService
  ) {}
  ngOnInit(): void {
    this.isUpdate = this.route.url.includes('project-update');
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'id',
      textField: 'visaFullName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 4,
      allowSearchFilter: true,
      maxHeight: 100,
    };
    this.employeeService.getAll().subscribe((res: ResponseDto) => {
      res.data.forEach((element: Employee) => {
        this.listEmployeeAll.push(element);
        this.dropdownList = this.employeeService.employeeToEmployeeDropdown(
          this.listEmployeeAll
        );
      });
    });
    if (this.isUpdate) {
      this.updateId = this.route.url.slice('/project-update/'.length);
      this.projectService
        .getProjectUpdate(parseInt(this.updateId))
        .subscribe((res: ResponseDto) => {
          this.project = res.data;
          res.data.endDate || (this.project.endDate = null);
          this.selectedItems = this.employeeService.employeeToEmployeeDropdown(
            this.project.employees
          );
          this.getForm.projectNumber.setValue(
            this.project.projectNumber!.toString()
          );
          this.getForm.projectName.setValue(this.project.name);
          this.getForm.customer.setValue(this.project.customer);
          this.getForm.groupId.setValue(this.project.groupId!.toString());
          this.getForm.status.setValue(this.project.status);
          this.getForm.startDate.setValue(
            new Date(this.project.startDate).toISOString().split('T')[0]
          );
          if (this.project.endDate) {
            this.getForm.endDate.setValue(
              new Date(this.project.endDate).toISOString().split('T')[0]
            );
          }
          this.getForm.version.setValue(this.project.version || '');
        });
      //this.createProjectForm.get('projectNumber')?.disable();
    }
  }

  //#region "Form and validate form"
  checkMaxLength(element: any, maxLength: number) {
    if (element.value) {
      const parentElement = element.parentElement;
      const spanWaring = parentElement.querySelector('.absolute');
      if (spanWaring && spanWaring.classList.contains('active')) {
        spanWaring.classList.remove('active');
      }
      if (element.value.length == maxLength) {
        this.messageLength = 'validMaxLength';
        spanWaring.classList.add('active');
      }
    }
  }
  continueInput() {
    this.validForm = true;
  }
  checkStartDate(element: any) {
    if (this.getForm.endDate.value && element.value) {
      const start = new Date(element.value);
      const end = new Date(this.getForm.endDate.value);
      if (end <= start) {
        this.validForm = false;
        this.formMessage = 'validDate';
      }
    }
  }
  checkEndDate(element: any) {
    if (this.getForm.startDate.value && element.value) {
      const start = new Date(this.getForm.startDate.value);
      const end = new Date(element.value);
      if (end <= start) {
        this.validForm = false;
        this.formMessage = 'validDate';
      }
    }
  }
  checkExistProjectNumber(element: any) {
    this.isChecking = true;
    if (element.value) {
      const projectNumber = Number.parseInt(element.value);
      this.projectService
        .checkProjectNumber(projectNumber)
        .subscribe((res: boolean) => {
          this.isExistProjectNumber = res;
          this.isChecking = false;
        });
    } else {
      this.isExistProjectNumber = false;
      this.isChecking = false;
    }
  }
  //create form
  createProjectForm = this.formBuilder.group({
    projectNumber: new FormControl('', [
      Validators.required,
      Validators.max(9999),
      Validators.min(1),
    ]),
    projectName: new FormControl('', [
      Validators.maxLength(50),
      Validators.required,
    ]),
    customer: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    groupId: new FormControl('', [Validators.required]),
    member: new FormControl([], [Validators.required]),
    status: new FormControl('NEW', [Validators.required]),
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl(''),
    version: new FormControl('')
  });
  get getForm() {
    return this.createProjectForm.controls;
  }
  checkDateValid(start?: string, end?: string): boolean {
    if (end && start) {
      const startDate = new Date(start);
      const endDate = new Date(end);
      return endDate > startDate;
    }
    return true;
  }
  checkValidForm(): boolean {
    let check =
      this.getForm.projectNumber.valid &&
      this.getForm.projectName.valid &&
      this.getForm.customer.valid &&
      this.getForm.groupId.valid &&
      this.getForm.status.valid &&
      this.getForm.startDate.valid;
    if (!check) {
      this.formMessage = 'inputFullBlank';
      this.validForm = check;
      return check;
    } else {
      const dateValid = this.checkDateValid(
        this.getForm.startDate.value?.toString(),
        this.getForm.endDate.value?.toString()
      )
        ? true
        : false;
      this.formMessage = 'validDate';
      this.validForm = dateValid;
      return dateValid;
    }
  }
  hiddenMessage() {
    this.validForm = !this.validForm;
  }
  //#endregion "From and validate form"
  createProject() {
    const valid = this.checkValidForm();
    if (valid) {
      this.isProcessing = true;
      const valueForm = this.createProjectForm.value;
      const project = new ProjectCreate();

      project.groupId = Number(valueForm.groupId);
      project.projectNumber = Number(valueForm.projectNumber);
      project.name = String(valueForm.projectName);
      project.customer = String(valueForm.customer);
      const employees = this.employeeService.employeeDropdownToEmployee(
        this.selectedItems,
        this.listEmployeeAll
      );
      project.employees = employees;
      project.status = String(valueForm.status);
      if (valueForm.startDate != undefined) {
        project.startDate = new Date(valueForm.startDate);
      }
      if (valueForm.endDate != undefined) {
        project.endDate = new Date(valueForm.endDate);
      }

      this.projectService
        .createProject(project)
        .pipe(
          catchError((error: HttpErrorResponse) => {
            console.log(error);
            let responseDto: ResponseDto = new ResponseDto(
              error,
              false,
              'You have error. Please try again!'
            );
            this.isProcessing = false;
            console.log(responseDto);
            return of(responseDto);
          })
        )
        .subscribe((res: ResponseDto) => {
          if (res.isSuccess) {
            this.route.navigate(['project-list']);
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Create project success',
              type: 'success',
            });
          } else {
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Oop, server error. Please choose another project number!',
              type: 'error',
              duration: 5000
            });
          }
          this.isProcessing = false;
        });
    }
  }
  updateProject() {
    const valid = this.checkValidForm();
    if (valid) {
      this.isProcessing = true;
      const valueForm = this.createProjectForm.value;
      const employees = this.employeeService.employeeDropdownToEmployee(
        this.getForm.member.value!,
        this.listEmployeeAll
      );
      let projectUpdate = new Project();
      // projectUpdate.projectNumber = this.project.projectNumber;
      projectUpdate.id = parseInt(this.updateId);
      projectUpdate.groupId = Number(valueForm.groupId);
      projectUpdate.projectNumber = Number(valueForm.projectNumber);
      projectUpdate.name = String(valueForm.projectName);
      projectUpdate.customer = String(valueForm.customer);
      projectUpdate.status = String(valueForm.status);
      projectUpdate.employees = employees;

      if (valueForm.startDate != undefined) {
        projectUpdate.startDate = new Date(valueForm.startDate);
      }
      if (valueForm.endDate != undefined) {
        projectUpdate.endDate = new Date(valueForm.endDate);
      }
      projectUpdate.version = valueForm.version || '';
      
      this.projectService.updateProject(projectUpdate).subscribe({
        next: (res: ResponseDto) => {
          if (res.isSuccess) {
            this.route.navigate(['project-list']);
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Update project success',
              type: 'success',
            });
          } else {
            this.route.navigate(['project-list']);
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Another people modified it. Please reload and try again',
              type: 'error',
            });
          }
          // this.isProcessing = false;
        },
        error: (err: any) => {
          console.log(err);
          const dto = new ResponseDto(null, false, err.message);
          this.toastService.toast({
            title: `Status: !`,
            message: err.message + err.status,
            type: 'success',
          });
          return of(dto);
        },
        complete: () => {
          this.isProcessing = false;
        },
      });
    }
  }
  cancelCreate() {
    this.route.navigate(['project-list']);
  }
}
