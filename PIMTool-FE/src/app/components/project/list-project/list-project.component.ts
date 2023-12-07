import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { project } from 'src/app/models/project';
import { ProjectService } from 'src/app/services/project.service';
import { FormControl } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';
import {
  ConfirmationService,
  MessageService,
  ConfirmEventType,
} from 'primeng/api';

@Component({
  selector: 'app-list-project',
  templateUrl: './list-project.component.html',
  styleUrls: ['./list-project.component.scss'],
  providers: [ConfirmationService, MessageService],
  encapsulation: ViewEncapsulation.None,
})
export class ListProjectComponent implements OnInit, OnDestroy {
  projects: project[] = [];
  error: string | null = null;
  pageIndex: number = 0;
  pageSize: number = 5;
  totalItems: number = 0;
  currentPage: number = 1;

  isSearching: boolean = false;

  selectedProjects: number[] = [];
  notDeleteProjects: number[] = [];

  private unsubscribe$ = new Subject<void>();

  // for Search
  searchControl: FormControl = new FormControl('');
  statusFilterControl: FormControl = new FormControl('');

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const savedSearchState = this.projectService.getSearchState();

    if (savedSearchState.searchTerm || savedSearchState.statusFilter) {
      this.searchControl.setValue(savedSearchState.searchTerm);
      this.statusFilterControl.setValue(savedSearchState.statusFilter);
      this.isSearching = true;

      if (savedSearchState.result.length > 0) {
        // Hiển thị kết quả từ trạng thái lưu
        this.projects = savedSearchState.result;
      } else {
        // Nếu kết quả không tồn tại trong trạng thái lưu, thực hiện tìm kiếm
        this.onSearch();
      }
    } else {
      this.getAllProject();
    }
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getAllProject() {
    this.projectService.getProject(this.pageIndex, this.pageSize).subscribe(
      (data: any) => {
        this.totalItems = data.totalItemsCount;
        console.log('Get all projects is called');
        this.projects = data.items;
      },
      (error: any) => {
        console.error('Error:', error);
        this.error = 'An error occurred while fetching project data.';
      }
    );
  }

  onPageChanged(event: any): void {
    this.currentPage = event;
    this.pageIndex = this.currentPage - 1;

    if (!this.isSearching) {
      this.getAllProject();
    } else {
      this.onSearch();
    }
  }

  onSearch() {
    const statusFilter = this.statusFilterControl.value;
    const searchTerm = this.searchControl.value;

    this.projectService
      .searchProject(searchTerm, statusFilter, this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (data: any) => {
          this.totalItems = data.totalItemsCount;
          this.projects = data.items;
          this.isSearching = true;

          this.projectService.setSearchState({
            searchTerm,
            statusFilter,
            result: data.items,
          });
        },
        (error: any) => {
          console.error('Error:', error);
          this.error = 'An error occurred while fetching project data.';
        }
      );
  }

  onResetSearch() {
    this.pageIndex = 0;
    this.currentPage = this.pageIndex + 1;
    this.isSearching = false;
    this.searchControl.setValue('');
    this.statusFilterControl.setValue('');
    this.selectedProjects = [];
    this.projectService.resetSearchState();
    this.getAllProject();
  }

  confirmDelete(projectId: number) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      accept: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Confirmed',
          detail: 'Record deleted',
        });
        console.log('onDelete Called');
        this.onDeleteProject(projectId);
      },
      reject: (type: any) => {
        switch (type) {
          case ConfirmEventType.REJECT:
            this.messageService.add({
              severity: 'error',
              summary: 'Rejected',
              detail: 'You have rejected',
            });
            console.log('Rejected call');
            break;
          case ConfirmEventType.CANCEL:
            this.messageService.add({
              severity: 'warn',
              summary: 'Cancelled',
              detail: 'You have cancelled',
            });
            console.log('Cancel call');
            break;
        }
      },
    });
  }

  confirmDeleteProjects() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      accept: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Confirmed',
          detail: 'Record deleted',
        });
        console.log('onDelete Called');
        this.onDeleteProjects();
      },
      reject: (type: any) => {
        switch (type) {
          case ConfirmEventType.REJECT:
            this.messageService.add({
              severity: 'error',
              summary: 'Rejected',
              detail: 'You have rejected',
            });
            console.log('Rejected call');
            break;
          case ConfirmEventType.CANCEL:
            this.messageService.add({
              severity: 'warn',
              summary: 'Cancelled',
              detail: 'You have cancelled',
            });
            console.log('Cancel call');
            break;
        }
      },
    });
  }

  onDeleteProject(projectId: number) {
    this.projectService.deleteProject(projectId).subscribe(
      (response) => {
        console.log('Project is deleted successfully');
        this.getAllProject();
      },
      (error) => {
        console.log('Error when delete project', error);
      }
    );
  }

  onDeleteProjects() {
    this.projectService.deleteProjects(this.selectedProjects).subscribe(
      (response) => {
        this.selectedProjects = [];
        console.log('Projects deleted successfully');
        this.getAllProject();
      },
      (error) => {
        console.log('Error when delete list projects', error);
      }
    );
  }

  toggleProjectSelection(projectId: number) {
    const index = this.selectedProjects.indexOf(projectId);

    const selectedProject = this.projects.find((p) => p.id === projectId);
    if (selectedProject && selectedProject.status === 0) {
      if (index === -1) {
        // if not found project id => push project into array
        this.selectedProjects.push(projectId);
      } else {
        // if have project id in array => remove this project
        this.selectedProjects.splice(index, 1);
      }
    } else {
      console.log('Cannot select project with status != NEW');
    }
  }

  navigateToProjectDetail(projectId: number) {
    console.log(projectId);
    this.router.navigate([`/edit/project/${projectId}`]);
  }
}
