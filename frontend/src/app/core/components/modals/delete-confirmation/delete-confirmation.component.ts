import {Component, EventEmitter, Input, Output} from '@angular/core';
import {faXmark} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-delete-confirmation',
  templateUrl: './delete-confirmation.component.html',
  styleUrls: ['./delete-confirmation.component.scss']
})
export class DeleteConfirmationComponent {
  @Input() projectName!: string
  @Input() isSingle!: boolean
  @Output() cancelEvent= new EventEmitter<void>()
  @Output() deleteEvent = new EventEmitter<void>()

  showModal = true

  hideModal() {
    this.showModal = false
  }

  cancel() {
    this.cancelEvent.emit()
  }

  delete() {
    this.deleteEvent.emit()
  }

  protected readonly faXmark = faXmark;
}
