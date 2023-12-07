import { Component, Output, EventEmitter, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observer, Subscription } from 'rxjs';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { FileConstants } from 'src/app/core/constants/fileConstants';
import { ProjectService } from 'src/app/core/services/project.service';
import { ApiResponse, ApiMessage } from 'src/app/core/services/project.service';
import { Router } from '@angular/router';
import { SubscriptionService } from 'src/app/core/services/subscription.service';
import { Store } from '@ngrx/store';
import { setLoadingOff, setLoadingOn } from 'src/app/core/store/page/page.actions';

@Component({
  selector: 'app-file-import',
  templateUrl: './file-import.component.html',
  styleUrls: ['./file-import.component.scss'],
})
export class FileImportComponent implements OnDestroy {

  constructor(
    private projectService: ProjectService,
    private toast: ToastrService,
    private router: Router,
    private subService: SubscriptionService,
    private store: Store
  ) {}

  @Output() hide: EventEmitter<void> = new EventEmitter()
  @ViewChild('file', {static:false}) fileInput!: ElementRef<HTMLInputElement>
  fileName: string = ''
  subscriptions: Subscription[] = []
  isValidData: boolean = true
  fileErrorDownload: string = ''

  apiObserver: Observer<ApiResponse> = {
    next: (response) => {
      setTimeout(() => {
        if (response.isSuccess){
          response.messages.forEach(msg => msg.type === 1 && this.toast.success(msg.content, 'Success'))
          this.router.navigate(['project/project-list'])
        } else {
          response.messages.forEach(
            (msg: ApiMessage) => 
              msg.content.split(',').forEach(
                content => this.toast.error(content, 'Error')
              )
          )
        }
        this.store.dispatch(setLoadingOff())
      }, 200)
    },
    error: (err: HttpErrorResponse) => {
      err.error.messages.forEach(
        (msg: ApiMessage) => 
          msg.content.split(',').forEach(
            content => {
              this.toast.error(content, 'Error')
            }
          )
      )
      this.store.dispatch(setLoadingOff())
    },
    complete: () => { }
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  handleFileChange(): void {
    const files = this.fileInput.nativeElement.files
    this.fileName = files ? files[0].name : ''
  }
  
  handleSubmit(): void {
    const files = this.fileInput.nativeElement.files
    if (!files || !files[0]) {
      return
    }
    
    this.store.dispatch(setLoadingOn())
    this.subscriptions.push(
      this.projectService.importProjectsFromFile(files[0]).subscribe((res: any) => {

        if (!res.size) {
          this.toast.success("Imported projects successfully", "Success")
          this.router.navigateByUrl("project/project-list")
        } else {
          this.isValidData = false
          const url = window.URL.createObjectURL(res)
          this.fileErrorDownload = url
        }

        this.store.dispatch(setLoadingOff())
      })
    )
  }
  
  downloadTemplate(): void {
    window.open(FileConstants.TEMPLATE_FILE_LOCATION)
  }

  faXmark = faXmark
}
