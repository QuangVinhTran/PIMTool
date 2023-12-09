import { SearchSortModel } from './../../search-sort-state/search-sort.model';
import { DatePipe } from '@angular/common';
import { ProjectService } from './../../../shared/services/project.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import {
  faTrashCan,
  faAnglesRight,
  faAnglesLeft,
  faArrowUp,
  faArrowDown,
  faSpinner,
  faLaptopHouse,
  // fa-spinner fa-pulse
} from '@fortawesome/free-solid-svg-icons';
import { Project } from 'src/shared/models/project';
import { ResponseDto } from 'src/shared/models/responseDto';
import { ToastService } from 'src/shared/services/toast.service';
import {
  selectCurrentIndexPage,
  selectSearchStatus,
  selectSearchText,
  selectSortCustomer,
  selectSortName,
  selectSortNumber,
  selectSortStartDate,
  selectSortStatus,
} from 'src/app/search-sort-state/search-sort.selector';
import { Store } from '@ngrx/store';
import {
  addSearchSort,
  clearSearchSort,
} from 'src/app/search-sort-state/search-sort.action';
import { catchError } from 'rxjs';
import { SearchSortState } from 'src/app/search-sort-state/search-sort-state';
@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css'],
})
export class ProjectListComponent implements OnInit {
  faTrashCan = faTrashCan;
  faAnglesRight = faAnglesRight;
  faAnglesLeft = faAnglesLeft;
  faArrowUp = faArrowUp;
  faArrowDown = faArrowDown;
  faSpinner = faSpinner;
  show: boolean = false;
  isLoading: boolean = false;
  // isAccept : boolean = false;

  DES = 'DES';
  ASC = 'ASC';
  projects: Project[] = [];
  arrayTotalPage: number[] = [];
  listRemoveId: number[] = [];
  pageSize = 10;
  currentPageIndex = this.searchSortState.currentPageIndex;
  sortNumber = this.searchSortState.sortName;
  sortName = this.searchSortState.sortName;
  sortStatus = this.searchSortState.sortStatus;
  sortCustomer = this.searchSortState.sortCustomer;
  sortStartDate = this.searchSortState.sortStartDate;
  searchText = this.searchSortState.searchText;
  searchStatus = this.searchSortState.searchStatus;
  errors : any;

  searchFrom = this.formBuilder.group({
    searchText: new FormControl(this.searchText),
    searchStatus: new FormControl(this.searchStatus),
  });

  initValueForm = this.searchFrom.value;

  constructor(
    private projectService: ProjectService,
    private formBuilder: FormBuilder,
    private toastService: ToastService,
    private router: Router,
    public datePipe: DatePipe,
    private store: Store,
    private searchSortState : SearchSortState
  ) {
    this.currentPageIndex = searchSortState.currentPageIndex;
    this.sortNumber = searchSortState.sortNumber,
    this.sortName =searchSortState.sortName,
    this.sortStatus = searchSortState.sortStatus,
    this.sortCustomer = searchSortState.sortCustomer,
    this.sortStartDate = searchSortState.sortStartDate,
    this.searchText = searchSortState.searchText,
    this.searchStatus = searchSortState.searchStatus
  }

  ngOnInit(): void {
    // this.loadLocalStorage();
    // this.movePage(this.currentPageIndex);

    //this.initLocalStorageSearchSort();
    console.log(this.searchSortState);
    
    this.movePage();
    localStorage.setItem('listRemoveId', JSON.stringify([]));
  }
  private loadLocalStorage() {
    this.store
      .select(selectSearchText)
      .subscribe((value) => (this.searchText = value));
    this.store
      .select(selectSearchStatus)
      .subscribe((value) => (this.searchStatus = value));
    this.store
      .select(selectSortNumber)
      .subscribe((value) => (this.sortNumber = value));
    this.store
      .select(selectSortName)
      .subscribe((value) => (this.sortName = value));
    this.store
      .select(selectSortStatus)
      .subscribe((value) => (this.sortStatus = value));
    this.store
      .select(selectSortCustomer)
      .subscribe((value) => (this.sortCustomer = value));
    this.store
      .select(selectSortStartDate)
      .subscribe((value) => (this.sortStartDate = value));
    this.store
      .select(selectCurrentIndexPage)
      .subscribe((value) => (this.currentPageIndex = value));
  }
  getArrayTotalPage(count: number) {
    for (let i = 1; i < count + 1; i++) {
      this.arrayTotalPage.push(i);
    }
  }
  //#region Search, reset search and sort, sort
  resetSearch() {
    this.initValueForm.searchStatus = "0";
    this.initValueForm.searchText = '';
    this.resetSort();
    this.searchFrom.reset(this.initValueForm);
    this.movePage(1);
  }
  searchProject() {
    this.resetSort();
    this.movePage(1);
  }
  sortByNumber() {
    if (this.sortNumber == '0') {
      this.resetSort();
    }
    this.sortNumber = this.sortNumber == this.ASC ? this.DES : this.ASC;
    this.searchSortState.sortNumber = this.sortNumber;
    this.movePage(1);
  }
  sortByName() {
    if (this.sortName == '0') {
      this.resetSort();
    }
    this.sortName = this.sortName == this.ASC ? this.DES : this.ASC;
    this.searchSortState.sortName = this.sortName;
    this.movePage(1);
  }
  sortByStatus() {
    if (this.sortStatus == '0') {
      this.resetSort();
    }
    this.sortStatus = this.sortStatus == this.ASC ? this.DES : this.ASC;
    this.searchSortState.sortStatus = this.sortStatus;
    this.movePage(1);
  }
  sortByCustomer() {
    if (this.sortCustomer == '0') {
      this.resetSort();
    }
    this.sortCustomer = this.sortCustomer == this.ASC ? this.DES : this.ASC;
    this.searchSortState.sortCustomer = this.sortCustomer;
    this.movePage(1);
  }
  sortByStartDate() {
    if (this.sortStartDate == '0') {
      this.resetSort();
    }
    this.sortStartDate = this.sortStartDate == this.ASC ? this.DES : this.ASC;
    this.searchSortState.sortStartDate = this.sortStartDate;
    this.movePage(1);
  }
  resetSort() {
    this.sortNumber = '0';
    this.searchSortState.sortNumber = '0';
    this.sortName = '0';
    this.searchSortState.sortName = '0';
    this.sortStatus = '0';
    this.searchSortState.sortStatus = '0';
    this.sortCustomer = '0';
    this.searchSortState.sortCustomer = '0';
    this.sortStartDate = '0';
    this.searchSortState.sortStartDate = '0';
  }
  //#endregion

  //#region  Paging
  getListFromLocalStorage() {
    const listLocalStorage = localStorage.getItem('listRemoveId');
    if (listLocalStorage) {
      this.listRemoveId = JSON.parse(listLocalStorage);
    }
  }
  movePage(pageIndex? : number) {
    this.isLoading = true;
    this.currentPageIndex = pageIndex || this.currentPageIndex;
    this.searchSortState.currentPageIndex = this.currentPageIndex;

    const searchValue = this.searchFrom.value;
    this.projects = [];
    this.arrayTotalPage = [];
    if (
      searchValue.searchStatus &&
      searchValue.searchText != null &&
      searchValue.searchText != undefined
    ) {
      this.searchSortState.searchText = searchValue.searchText;
      this.searchSortState.searchStatus = searchValue.searchStatus;

      this.projectService
        .pagingProject(
          this.pageSize,
          this.currentPageIndex,
          searchValue.searchText,
          searchValue.searchStatus,
          this.sortNumber,
          this.sortName,
          this.sortStatus,
          this.sortCustomer,
          this.sortStartDate
        )
        .subscribe({
          next: (res: ResponseDto) => {
            res.data.result.forEach((value: Project) => {
              if (value.startDate) {
                value.startDate = new Date(value.startDate);
              }
              if (value.endDate) {
                value.endDate = new Date(value.endDate);
              }
              this.projects.push(value);
            });
            this.getArrayTotalPage(res.data.totalPage);
            this.isLoading = false;
          },
          error: (error) => {
            console.log(error);
          },
          complete: () => {},
        });
    }
  }
  //#endregion

  //#region Remove project
  removeMultiple() {
    this.show = true;
    console.log('multiple');
    this.singleOrMultiple = 'multiple';
  }
  removeRange() {
    const listLocalStorage = localStorage.getItem('listRemoveId');
    if (listLocalStorage) {
      const list = JSON.parse(listLocalStorage);
      this.projectService
        .removeProjectRange(list)
        .subscribe((res: ResponseDto) => {
          if (res.isSuccess) {
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Delete range project success',
              type: 'success',
            });
            localStorage.setItem('listRemoveId', JSON.stringify([]));
            this.listRemoveId = [];
            this.movePage(this.currentPageIndex);
          } else {
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: `${res.error}`,
              type: 'error',
            });
          }
        });
    }
  }

  clickProject(projectId?: number) {
    if (projectId) {
      const checkExist = this.listRemoveId.indexOf(projectId);
      if (checkExist !== -1) {
        this.listRemoveId.splice(checkExist, 1);
        localStorage.setItem('listRemoveId', JSON.stringify(this.listRemoveId));
      } else {
        this.listRemoveId.push(projectId);
        localStorage.setItem('listRemoveId', JSON.stringify(this.listRemoveId));
      }
    }
  }
  singleOrMultiple: string = '';
  id: any;
  removeSingle(id?: number) {
    this.show = true;
    this.singleOrMultiple = 'single';
    this.id = id;
  }
  removeProject(projectId?: number) {
    if (projectId) {
      this.projectService
        .removeProject(projectId)
        .subscribe((res: ResponseDto) => {
          if (res.isSuccess) {
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: 'Delete project success',
              type: 'success',
            });
            const checkExist = this.listRemoveId.indexOf(projectId);
            if (checkExist !== -1) {
              this.listRemoveId.splice(checkExist, 1);
              localStorage.setItem(
                'listRemoveId',
                JSON.stringify(this.listRemoveId)
              );
            }
            this.movePage(this.currentPageIndex);
          } else {
            this.toastService.toast({
              title: `Status: ${res.isSuccess}!`,
              message: `${res.error}`,
              type: 'error',
            });
          }
        });
    }
  }
  //#endregion

  // #region update project
  updateProject(projectId: number) {
    this.router.navigate([`project-update/${projectId}`]);
  }
  //#endregion
}
