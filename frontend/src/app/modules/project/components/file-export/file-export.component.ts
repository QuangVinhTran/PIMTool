import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { Store } from '@ngrx/store';
import { ProjectService } from 'src/app/core/services/project.service';
import { setLoadingOff, setLoadingOn } from 'src/app/core/store/page/page.actions';

@Component({
  selector: 'app-file-export',
  templateUrl: './file-export.component.html',
  styleUrls: ['./file-export.component.scss']
})
export class FileExportComponent {

  constructor(
    private projectService: ProjectService,
    private store: Store
  ) { }

  @Output() hide: EventEmitter<void> = new EventEmitter()
  request: ExportFileRequest = {
    projectName: '',
    customer: '',
    leaderName: '',
    startDateFrom: '',
    startDateTo: '',
    endDateFrom: '',
    endDateTo: '',
    status: '',
    orderBy: 'projectNumber',
    numberOfRows: 10,
  }

  export(): void {
    this.store.dispatch(setLoadingOn())

    const clonedRequest = {...this.request}
    !clonedRequest.startDateFrom ? clonedRequest.startDateFrom = '0001-01-01' : () => {}
    !clonedRequest.startDateTo ? clonedRequest.startDateTo = '0001-01-01' : () => {}
    !clonedRequest.endDateFrom ? clonedRequest.endDateFrom = '0001-01-01' : () => {}
    !clonedRequest.endDateTo ? clonedRequest.endDateTo = '0001-01-01' : () => {}

    this.projectService.exportProjectsToFile(clonedRequest).subscribe((res: any) => {
      const url= window.URL.createObjectURL(res);
      const link = document.createElement('a')
      link.href = url
      link.download = 'Projects.xlsx'
      link.click()
      document.removeChild(link)
      this.store.dispatch(setLoadingOff())
    })
  }

  faXmark = faXmark
}

export interface ExportFileRequest {
  projectName: string
  customer: string
  leaderName: string
  startDateFrom: string
  startDateTo: string
  endDateFrom: string
  endDateTo: string
  status: string
  orderBy: string
  numberOfRows: number
}
