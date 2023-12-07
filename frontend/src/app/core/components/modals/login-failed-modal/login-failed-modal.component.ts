import {Component, Input} from '@angular/core';
import {faXmark} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-login-failed-modal',
  templateUrl: './login-failed-modal.component.html',
  styleUrls: ['./login-failed-modal.component.scss']
})
export class LoginFailedModalComponent {
  @Input() message: string = 'Invalid password'
  @Input() showModal: boolean = true

  hideModal() {
    this.showModal = false
  }

  protected readonly faXmark = faXmark;
}
