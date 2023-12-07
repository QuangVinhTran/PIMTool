import {Component, OnInit, OnDestroy} from '@angular/core';
import {faTrash} from "@fortawesome/free-solid-svg-icons";
import {Store} from "@ngrx/store";
import {Project, ProjectToDelete} from "../../../core/models/project/project.models";
import {ApiResponse, ProjectService} from "../../../core/services/project.service";
import {PaginationStatus, SearchCriteria, SortInfo} from "../../../core/models/filter.models";
import {addSortInfo, revertSortOrder} from "../../../core/store/sort/sort.actions";
import {selectFilterStatus} from "../../../core/store/advanced-filter/advancedFilter.selectors";
import {selectSortInfo} from "../../../core/store/sort/sort.selectors";
import {collapseAnimation} from "../../../core/animations/collapse.animation";
import { slideAnimation } from 'src/app/core/animations/slide.animation';
import {selectAllSearch} from "../../../core/store/search/search.selectors";
import { selectAllowExportFile, selectAllowImportFile } from 'src/app/core/store/setting/setting.selectors';
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss'],
  animations: [...collapseAnimation, ...slideAnimation]
})
export class ProjectListComponent implements OnInit, OnDestroy {

  projects: Project[] = []
  selectedProjects: string[] = []
  isLoading: boolean = true
  showDeleteModal: boolean = false
  singleDeletion: boolean = true
  showAdvancedFilter: boolean = false
  allowExportFile: boolean = false
  exportFile: boolean = false
  searchCriteria!: SearchCriteria
  sortInfo!: SortInfo
  subscriptions: Subscription[] = []
  paginationStatus: PaginationStatus = {
    pageIndex: 1,
    pageSize: 10,
    lastPage: 1,
    isLastPage: false,
  }
  projectToDelete: ProjectToDelete = {
    id: '',
    name: ''
  }

  constructor(
    private store: Store,
    private projectService: ProjectService,
    private subService: SubscriptionService
  ) { }
  
  ngOnInit() {
    const allSearchSub = this.store.select(selectAllSearch).subscribe(value => this.searchCriteria = value)
    const sortSub = this.store.select(selectSortInfo).subscribe(value => this.sortInfo = value)
    const filterStatusSub = this.store.select(selectFilterStatus).subscribe(value => this.showAdvancedFilter = value)
    const allowExportFileSub = this.store.select(selectAllowExportFile).subscribe(value => this.allowExportFile = value)
    this.setProjects()

    this.subscriptions.push(allSearchSub)
    this.subscriptions.push(sortSub)
    this.subscriptions.push(filterStatusSub)
    this.subscriptions.push(allowExportFileSub)
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  selectProject(projectId: string): void {
    this.isProjectSelected(projectId)
      ? this.selectedProjects.splice(this.selectedProjects.indexOf(projectId),1)
      : this.selectedProjects.push(projectId)
  }

  setProjects() {
    this.isLoading = true
    const projectSub = this.projectService.loadProjects(this.paginationStatus.pageIndex, this.paginationStatus.pageSize, this.searchCriteria, this.sortInfo)
      .subscribe(value => {
        this.projects = value.data?.data
        this.paginationStatus = {
          ...this.paginationStatus,
          pageIndex: value.data?.pageIndex,
          lastPage: value.data?.lastPage,
          isLastPage: value.data?.isLastPage
        }
        this.isLoading = false
      })
      
    this.subscriptions.push(projectSub)
  }

  isProjectSelected(projectId: string): boolean {
    return this.selectedProjects.includes(projectId)
  }

  addSort(fieldName: string) {
    this.sortInfo.fieldName === fieldName
      ? this.store.dispatch(revertSortOrder({fieldName}))
      : this.store.dispatch(addSortInfo({fieldName}))

    this.setProjects()
  }

  selectProjectToDelete(id: string, name: string) {
    this.projectToDelete = { id, name }
    this.singleDeletion = true
    this.showDeleteModal = true
  }

  deleteSingleProject() {
    this.showDeleteModal = false
    this.projectService.deleteProject(this.projectToDelete.id)
    this.selectedProjects.splice(this.selectedProjects.indexOf(this.projectToDelete.id), 1)
    this.setProjects()
  }

  deleteMultipleProjects() {
    this.showDeleteModal = false
    this.projectService.deleteMultipleProjects(this.selectedProjects)
    this.selectedProjects = []
    this.setProjects()
  }

  cancelDelete() {
    this.showDeleteModal = false
  }

  // exportFile(): void {
  //   this.projectService.exportProjectsToFile().subscribe((value: any) => {

  //     const url= window.URL.createObjectURL(value);
  //     const link = document.createElement('a')
  //     link.href = url
  //     link.download = 'Projects.xlsx'
  //     link.click()
  //   })
  // }

  faTrash = faTrash
}
