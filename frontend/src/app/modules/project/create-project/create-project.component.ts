import {Component, OnInit, OnDestroy} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {faXmark} from "@fortawesome/free-solid-svg-icons";
import {Router} from "@angular/router";
import {GroupService} from "../../../core/services/group.service";
import {formatDateTime} from "../../../core/utils/date.util";
import {ApiMessage, ApiResponse, ProjectService} from "../../../core/services/project.service";
import {Group} from "../../../core/models/project/project.models";
import {routes} from "../../../core/constants/routeConstants";
import { Observer, Subscription } from "rxjs";
import {ToastrService} from "ngx-toastr";
import {Employee} from "../../../core/models/project/employee.model";
import {EmployeeService} from "../../../core/services/employee.service";
import {HttpErrorResponse} from "@angular/common/http";
import { Store } from '@ngrx/store';
import { selectAllowImportFile } from 'src/app/core/store/setting/setting.selectors';
import { slideAnimation } from 'src/app/core/animations/slide.animation';
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { setLoadingOff, setLoadingOn } from 'src/app/core/store/page/page.actions';
import { selectLoadingState } from 'src/app/core/store/page/page.selectors';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss'],
  animations: [...slideAnimation]
})
export class CreateProjectComponent implements OnInit, OnDestroy {

  isLoading = false
  isSuccess = false
  isValidProjectNumber = true
  isRequestSent = false
  doCreate = true
  focus = false
  mouseIn = false
  showErrorMsg = true
  allowImportFile: boolean = false
  importFile: boolean = false

  message = ''
  groups: Group[] = []
  projectId = ''
  projectVersion = 0
  members: Employee[] = []
  filteredMembers: Employee[] = []
  selectedMembers: Employee[] = []
  subscriptions: Subscription[] = []

  constructor(
    private formBuilder: FormBuilder,
    protected router: Router,
    protected groupService: GroupService,
    protected projectService: ProjectService,
    protected employeeService: EmployeeService,
    private toast: ToastrService,
    private store: Store,
    private subService: SubscriptionService
  ) { }

  apiObserver: Observer<ApiResponse> = {
    next: (response) => {
      setTimeout(() => {
        if (response.isSuccess){
          response.messages.forEach(msg => msg.type === 1 && this.toast.success(msg.content, 'Success'))
          this.store.dispatch(setLoadingOff())
          this.router.navigate(['project/project-list'])
        } else {
          response.messages.forEach(msg => this.toast.error(msg.content, 'Error'))
        }
      }, 200)
    },
    error: (err: HttpErrorResponse) => {
      this.store.dispatch(setLoadingOff())
      err.error.messages.forEach((msg: ApiMessage) => this.toast.error(msg.content, 'Error'))
    },
    complete: () => {}
  }

  createProjectForm = this.formBuilder.group({
    projectNumber:  new FormControl(0, [
      Validators.required,
      Validators.min(0),
      Validators.max(9999),
    ]),
    name: new FormControl('', [
      Validators.required,
    ]),
    customer: new FormControl('', [
      Validators.required,
    ]),
    groupId: new FormControl('', [
      Validators.required,
    ]),
    members: '',
    status: new FormControl('NEW', [
      Validators.required,
    ]),
    startDate: new FormControl('', [
      Validators.required,
    ]),
    endDate: ''
  })

  ngOnInit() {
    const importFileSub = this.store.select(selectAllowImportFile).subscribe(value => this.allowImportFile = value)
    const groupSub = this.groupService.getGroups().subscribe(value => this.groups = value)
    const employeeSub = this.employeeService.getEmployees()
      .subscribe(value => {
        this.members = [...value]
        this.filteredMembers = [...value]
      })
    const loadingSub = this.store.select(selectLoadingState).subscribe(value => this.isLoading = value)
    const url = this.router.url
    if (url === routes.CREATE_PROJECT) {
      this.doCreate = true
    } else {
      this.doCreate = false
      const id = url.slice(routes.UPDATE_PROJECT.length + 1, url.length)
      this.projectService.getSingleProject(id).subscribe(project => {
        this.createProjectForm.setValue({
          projectNumber: project.projectNumber,
          name: project?.name,
          customer: project?.customer,
          groupId: project?.groupId || '',
          members: '',
          status: project?.status,
          startDate: formatDateTime(project?.startDate),
          endDate: (project && project?.endDate) ? formatDateTime(project?.endDate) : null
        })
        this.projectId = project?.id
        this.projectVersion = project?.version
        this.selectedMembers = project?.employees || this.selectedMembers
      })
      this.createProjectForm.controls['projectNumber'].disable()
    }

    this.subscriptions.push(importFileSub)
    this.subscriptions.push(groupSub)
    this.subscriptions.push(employeeSub)
    this.subscriptions.push(loadingSub)
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  onSubmit() {
    if (!this.createProjectForm.valid || !this.isValidProjectNumber) {
      return
    }

    // this.isLoading = true
    this.store.dispatch(setLoadingOn())
    this.doCreate
      ? this.projectService.createProject(this.createProjectForm.getRawValue())
        .subscribe(this.apiObserver)
      : this.projectService.updateProject(
        this.projectId,
        {
          ...this.createProjectForm.getRawValue(),
          memberIds: this.selectedMembers.map(e => e.id)
        },
        this.projectVersion
      ).subscribe(this.apiObserver)
  }

  hideErrorMsg(): void {
    this.showErrorMsg = false
  }

  validateProjectNumber() {
    this.projectService.validateProjectNumber(this.createProjectForm.get('projectNumber')?.getRawValue())
      .subscribe(value => this.isValidProjectNumber = value)
  }

  selectMember(memberId: string) {
    this.members.filter(e => e.id === memberId)
      .forEach(e => {
        this.selectedMembers.push(e)
        this.members.splice(this.members.indexOf(e), 1)
        this.filteredMembers.splice(this.filteredMembers.indexOf(e), 1)})
  }

  deselectMember(memberId: string) {
    this.selectedMembers.filter(e => e.id === memberId)
      .forEach(e => {
        this.members.push(e)
        this.filteredMembers.push(e)
        this.selectedMembers.splice(this.selectedMembers.indexOf(e), 1)
      })
  }

  filterMember() {
    const kw = this.createProjectForm.get('members')?.value
    this.filteredMembers = kw
      ? this.filteredMembers = this.filteredMembers.filter(e=>
        e.firstName.toUpperCase().includes(kw.toUpperCase().trim()) ||
        e.lastName.toUpperCase().includes(kw.toUpperCase().trim()))
      : this.filteredMembers = this.members
  }

  protected readonly faXmark = faXmark;
}
