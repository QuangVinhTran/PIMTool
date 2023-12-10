import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project } from '../models/project.model';
import { ProjectService } from '../services/project.service';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { Paginator, PaginatorModule } from 'primeng/paginator';
import {
  ActivatedRoute,
  Params,
  Router,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DeleteDialogComponent } from 'src/app/components/delete-dialog/delete-dialog.component';
import { switchMap } from 'rxjs';
import { GlobalState } from 'src/app/SearchValueRx/global-state.model';
import { Store } from '@ngrx/store';
import {
  selectSearchValue,
  selectStatus,
} from 'src/app/SearchValueRx/global-state.selectors';
import {
  setSearchValue,
  setStatus,
} from 'src/app/SearchValueRx/global-state.action';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [
    CommonModule,
    DropdownModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    RouterLinkActive,
    DialogModule,
    ButtonModule,
    PaginatorModule,
  ],
  providers: [DialogService],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.scss',
})
export class ProjectListComponent implements OnInit {
  projectList!: Project[];
  searchForm!: FormGroup;
  isOpenDialog: boolean = false;
  dialogRef: DynamicDialogRef | undefined;
  page: number = 1;
  totalProject: number = 0;
  pageSize: number = 5;
  checkedDeleteProject: number[] = [];

  constructor(
    private projectService: ProjectService,
    private fb: FormBuilder,
    public dialogService: DialogService,
    private store: Store<{ globalState: GlobalState }>
  ) {
    this.searchForm = this.fb.group({
      searchValue: '',
      status: '',
    });
  }
  ngOnInit(): void {
    this.store.select(selectSearchValue).subscribe((searchValue) => {
      this.searchForm.patchValue({ searchValue });
    });

    this.store.select(selectStatus).subscribe((status) => {
      this.searchForm.patchValue({ status });
    });
    this.projectService
      .SearchProject(
        this.searchForm.value.searchValue,
        this.searchForm.value.status,
        this.page,
        this.pageSize
      )
      .subscribe((data) => {
        this.projectList = data.items;
        this.totalProject = data.totalCount;
        this.pageSize = data.pageSize;
      });
  }

  statusConvert: any = {
    NEW: 'New',
    PLA: 'Planned',
    INP: 'In Progress',
    FIN: 'Finished',
  };

  statusList: any[] = [
    { name: '-- None --', value: '' },
    { name: 'New', value: 'NEW' },
    { name: 'Planned', value: 'PLA' },
    { name: 'In progress', value: 'INP' },
    { name: 'Finished', value: 'FIN' },
  ];

  extractDateComponents(date: Date): string {
    const dateTime = new Date(date);
    const day = dateTime.getDate();
    const month = dateTime.getMonth() + 1;
    const year = dateTime.getFullYear();
    return `${day}.${month}.${year}`;
  }

  onFormSubmit() {
    this.store.dispatch(
      setSearchValue({
        searchValue: this.searchForm.value.searchValue,
      })
    );
    this.store.dispatch(setStatus({ status: this.searchForm.value.status }));
    this.projectService
      .SearchProject(
        this.searchForm.value.searchValue,
        this.searchForm.value.status,
        this.page,
        this.pageSize
      )
      .subscribe((data) => {
        this.projectList = data.items;
        this.totalProject = data.totalCount;
        this.pageSize = data.pageSize;
      });
  }

  resetSearch() {
    this.store.dispatch(
      setSearchValue({
        searchValue: '',
      })
    );
    this.store.dispatch(setStatus({ status: '' }));
    this.searchForm.patchValue({
      searchValue: '',
      status: '',
    });
    this.page = 1;
    this.projectService
      .SearchProject(
        this.searchForm.value.searchValue,
        this.searchForm.value.status,
        this.page,
        this.pageSize
      )
      .subscribe((data) => {
        this.projectList = data.items;
        this.totalProject = data.totalCount;
        this.pageSize = data.pageSize;
      });
  }

  navigateUpdateProject(id: number) {
    console.log(id);
  }

  showDialog(projectNumber: number[]) {
    this.dialogRef = this.dialogService.open(DeleteDialogComponent, {
      header: 'Confirm delete project',
      width: '40%',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
    });

    this.dialogRef.onClose.subscribe((result) => {
      if (result === true) {
        this.projectService
          .DeleteProject(projectNumber)
          .pipe(
            switchMap(() =>
              this.projectService.GetAllProject(this.page, this.pageSize)
            )
          )
          .subscribe((data) => {
            this.projectList = data.items;
            this.totalProject = data.totalCount;
            this.pageSize = data.pageSize;
          });
        this.checkedDeleteProject = [];
      }
    });
  }
  onPageChange(event: any) {
    this.page = event.page + 1;
    this.projectService
      .SearchProject(
        this.searchForm.value.searchValue,
        this.searchForm.value.status,
        this.page,
        this.pageSize
      )
      .subscribe((data) => {
        this.projectList = data.items;
        this.totalProject = data.totalCount;
        this.pageSize = data.pageSize;
      });
  }

  handleCheckedDelete() {
    const deleteProjects = this.projectList.filter(
      (project) => project.checked
    );
    this.checkedDeleteProject = deleteProjects.map(
      (project) => project.projectNumber
    );
  }
}
