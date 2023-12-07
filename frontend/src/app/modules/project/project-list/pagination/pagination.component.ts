import {Component, EventEmitter, Input, Output} from '@angular/core';
import {
  faAnglesLeft,
  faAnglesRight,
  faChevronLeft,
  faChevronRight,
  faEllipsis
} from "@fortawesome/free-solid-svg-icons";
import {Project} from "../../../../core/models/project/project.models";
import {PaginationStatus} from "../../../../core/models/filter.models";

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {

  @Input() paginationStatus!: PaginationStatus
  @Output() childEvent = new EventEmitter<void>()

  toNextPage() {
    if (this.paginationStatus.pageIndex < this.paginationStatus.lastPage) {
      this.paginationStatus.pageIndex++;
    }

    this.childEvent.emit()
  }

  toNextMultiPage(): void {
    this.paginationStatus.pageIndex = Math.min(this.paginationStatus.pageIndex + 5, this.paginationStatus.lastPage)

    this.childEvent.emit()
  }

  toPreviousPage(): void {
    if (this.paginationStatus.pageIndex > 1){
      this.paginationStatus.pageIndex--;
    }

    this.childEvent.emit()
  }

  toPrevMultiPage(): void {
    this.paginationStatus.pageIndex = Math.max(this.paginationStatus.pageIndex - 5, 1)

    this.childEvent.emit()
  }

  setPage(index: number): void {
    this.paginationStatus.pageIndex = index

    this.childEvent.emit()
  }

  changeNextMultiIcon(): void  {
    this.nextMultiPageIcon = faAnglesRight
  }

  changePrevMultiIcon(): void  {
    this.prevMultiPageIcon = faAnglesLeft
  }


  resetIcon(): void {
    this.nextMultiPageIcon = faEllipsis
    this.prevMultiPageIcon = faEllipsis
  }


  protected readonly faChevronRight = faChevronRight;
  protected readonly faChevronLeft = faChevronLeft;
  protected nextMultiPageIcon = faEllipsis;
  protected prevMultiPageIcon = faEllipsis;
  protected readonly Math = Math;
}
