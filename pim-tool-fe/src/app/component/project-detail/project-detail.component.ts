import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ProjectService } from 'src/app/service/project.service';
import { Project, Status } from 'src/app/model/project';
import { GroupService } from '../../service/group.service';
import { Group } from 'src/app/model/group';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from 'src/app/service/shared.service';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { EmployeeService } from 'src/app/service/employee.service';
import { ProjectMembers } from '../../model/project';
import { formatDate } from '@angular/common';
import { Employee } from '../../model/employee';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.scss'],
})

export class ProjectDetailComponent {
  @ViewChild('alertPopup') alertPopup!: ElementRef;
  siteLanguage = 'English';
  languageList = [
    { code: 'en', label: 'English' },
    { code: 'fr', label: 'French' },
  ];
  groups: Group[] | undefined;
  updateProject!: Project;
  updateProjectMembers!: ProjectMembers;
  actionTitle: any = 'projectDetail.create.title';
  btnSubmitContent: any = 'projectDetail.create.btnCreate';
  editMode: boolean = false;
  numberErr: string = '';
  ennDateErr: string = '';
  globalErr: string = 'projectDetail.globalError';
  membersError: string = '';
  projectSent!: ProjectMembers;
  isFailed: boolean = false;
  empList: Employee[] = [];
  selectedItem: Employee[] = [];
  emptyMessage: "No employees found" | undefined;
  formGroup: FormGroup | undefined;
  selectedEmployee: number[] = [];
  hasEmp: boolean = false;

  constructor(
    private projectService: ProjectService,
    private groupService: GroupService,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    public sharedService: SharedService,
    private translate: TranslateService,
    private primengConfig: PrimeNGConfig
  ) { }

  ngOnInit(): void {
    this.primengConfig.ripple = true;
    this.getGroups();
    this.getEmployees();
    this.globalErr = 'projectDetail.globalError';
    const projectNumber: any =
      this.route.snapshot.paramMap.get('projectNumber');
    if (projectNumber) {
      this.editMode = !this.editMode;
      this.actionTitle = 'projectDetail.update.title';
      this.btnSubmitContent = 'projectDetail.update.btnUpdate';
      this.getProjectByNumber(projectNumber);
    }
    console.log('editMode: ', this.editMode);
  }

  getEmployees() {
    this.employeeService.getEmployees().subscribe(
      (response) => {
        this.hasEmp = true;
        this.empList = response
        console.log("list employees: ", this.empList);
      },
      (error: HttpErrorResponse) => {
        this.empList = []
        this.hasEmp = false;
        console.log("error get employees: ", error);
        this.navigateToErrorPage();
      }
    )
  }

  search($event: any) {
    this.employeeService.searchEmployees($event.query).subscribe(
      (response) => {
        this.empList = response,
        this.membersError = ''
      },
      (error: HttpErrorResponse) => {
        this.empList = []
      }
    )
  }

  selectEmpId(value: any) {
    console.log("select value: ", value);

    if (!this.selectedEmployee.includes(value.id)) {
      this.selectedEmployee.push(value.id);
    }

    console.log("selectedEmp: ", this.selectedEmployee);
  }

  unselectEmpId(value: any) {
    console.log("unselect value: ", value);
    const index = this.selectedEmployee.indexOf(value.id);

    if (index !== -1) {
      this.selectedEmployee.splice(index, 1);
    }

    console.log("selectedEmp: ", this.selectedEmployee);
  }

  changeSiteLanguage(localeCode: string): void {
    const selectedLanguage = this.languageList
      .find((language) => language.code === localeCode)
      ?.label.toString();
    if (selectedLanguage) {
      this.siteLanguage = selectedLanguage;
      this.translate.use(localeCode);
    }
    const currentLanguage = this.translate.currentLang;
    console.log('currentLanguage', currentLanguage);
  }

  public getProjectByNumber(projectNumber: string): void {
    this.projectService.getProjectByNumber(parseInt(projectNumber)).subscribe(
      (response: any) => {
        this.updateProjectMembers = response;
        this.selectedEmployee = response.listEmpId;
        var date = new Date(response.projectDto.startDate);
        response.projectDto.startDate = this.formatDateAfterLoadFromDb(date);
        if (response.projectDto.endDate) {
          date = new Date(response.projectDto.endDate);
          response.projectDto.endDate = this.formatDateAfterLoadFromDb(date);
        }
        this.updateProject = response.projectDto;
        console.log("Current project members: ", this.updateProjectMembers);
        console.log("test proj: ", this.updateProject);
        console.log("test mem: ", this.selectedEmployee);
        console.log("version: ", this.updateProject.version);

        // find the members from the empList that has the correct id from listEmpId and add to the selectedItem
        response.listEmpId.forEach((e: number) => {
          this.empList.forEach(emp => {
            if (e == emp.id) {
              this.selectedItem.push(emp);
            }
          })
        });

        console.log("selectedItem: ", this.selectedItem);
      },
      (error: HttpErrorResponse) => {
        console.log("error get project by project number: ", error);
        this.navigateToErrorPage();
      }
    );
  }

  private formatDateAfterLoadFromDb(date: any): any {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return (`${year}-${month}-${day}`);
  }

  public getGroups(): void {
    this.groupService.getGroups().subscribe(
      (response: Group[]) => {
        this.groups = response;
        console.log("list groups: ", this.groups);
      },
      (error: HttpErrorResponse) => {
        console.log("error get groups: ", error);
        this.navigateToErrorPage();
      }
    );
  }

  public onAddProject(addForm: NgForm): void {
    console.log(addForm.value);

    if (addForm.invalid) {
      this.globalErr = 'projectDetail.globalError';
      this.numberErr = '';
      return;
    }

    const startTime = new Date(addForm.value.startDate);
    console.log("startTime: ", startTime);

    const currentTime = new Date();
    currentTime.setHours(6, 0, 0, 0);
    console.log("currentTime: ", currentTime);

    if (startTime < currentTime) {
      this.ennDateErr = 'projectDetail.startBeforeCurrent';
      return;
    }

    if (addForm.value.endDate != null) {
      const endTime = new Date(addForm.value.endDate);

      if (startTime >= endTime) {
        this.ennDateErr = 'projectDetail.startAfterEnd';
        return;
      }
    }

    if (this.empList.length == 0 && this.hasEmp == true) {
      this.membersError = 'projectDetail.membersError';
      return;
    }

    var ProjectMembers: ProjectMembers;
    ProjectMembers = {
      ProjectDto: addForm.value,
      ListEmpId: this.selectedEmployee
    }

    this.projectService.addProject(ProjectMembers).subscribe(
      (response: any) => {
        console.log("list projects: ", response);
        addForm.reset();
        this.router.navigateByUrl('/list');
      },
      (error: HttpErrorResponse) => {
        console.log(error);
        console.log(error.error.detail);
        console.log("status text", error.statusText);

        if (error.error.detail.includes('project number already existed')) {
          this.numberErr = 'projectDetail.numberExist';
          this.isFailed = true;
          this.globalErr = 'projectDetail.createProjectFailed';
          return;
        }

        console.log("error add project: ", error);
        this.navigateToErrorPage();
      }
    );
  }

  public onUpdateProject(addForm: NgForm): void {
    console.log("Before checking update project: ", addForm.value);

    if (addForm.invalid) {
      this.globalErr = 'projectDetail.globalError';
      this.numberErr = '';
      return;
    }

    const startTime = new Date(addForm.value.startDate);

    const currentTime = new Date();
    currentTime.setHours(6, 0, 0, 0);

    if (startTime < currentTime) {
      this.ennDateErr = 'projectDetail.startBeforeCurrent';
      return;
    }

    if (addForm.value.endDate != null) {
      const endTime = new Date(addForm.value.endDate);

      if (startTime >= endTime) {
        this.ennDateErr = 'projectDetail.startAfterEnd';
        return;
      }
    }

    if (this.empList.length == 0 && this.hasEmp == true) {
      this.membersError = 'projectDetail.membersError';
      return;
    }

    var ProjectMembers: ProjectMembers;
    ProjectMembers = {
      ProjectDto: addForm.value,
      ListEmpId: this.selectedEmployee
    }

    //set the version of current project for the project sent to BE, cuz form's values dont contain version
    this.projectSent = ProjectMembers;
    console.log("before set version: ", this.projectSent.ProjectDto.version);
    this.projectSent.ProjectDto.version = this.updateProject.version;
    this.projectSent.ProjectDto.id = this.updateProject.id;
    console.log("loading version: ", this.updateProject.version);
    console.log("sending version: ", this.projectSent.ProjectDto.version);


    console.log('Updating values: ', this.projectSent);

    this.projectService.updateProject(this.projectSent).subscribe(
      (response: Project) => {
        console.log('Updated project: ', response);
        this.getGroups();
        this.router.navigateByUrl('/list');
        addForm.reset();
      },
      (error: HttpErrorResponse) => {
        console.log(error);
        console.log(error.error.detail);
        console.log("status text", error.statusText);

        this.isFailed = true;
        if (error.error.detail.includes('The project has been updated by another user')) {
          this.globalErr = 'projectDetail.concurrentUpdate';
          return;
        }

        console.log("error update project: ", error);
        this.navigateToErrorPage();
      }
    );
  }

  closeAlert() {
    this.alertPopup.nativeElement.style.display = 'none';
  }

  public navigateToList() {
    this.router.navigateByUrl('/list');
  }

  navigateToErrorPage() {
    this.router.navigate(['/error']);
  }
}
