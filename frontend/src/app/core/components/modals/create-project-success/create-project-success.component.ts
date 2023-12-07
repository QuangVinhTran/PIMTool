import {Component, Input} from '@angular/core';
import {faXmark} from "@fortawesome/free-solid-svg-icons";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-project-success',
  templateUrl: './create-project-success.component.html',
  styleUrls: ['./create-project-success.component.scss']
})
export class CreateProjectSuccessComponent {
  @Input() showModal: boolean = false
  @Input() isSuccess: boolean = false
  @Input() message: string = ''

  constructor(private router: Router) {
  }

  hideModal() {
    this.showModal = false
    if (this.isSuccess) {
      this.router.navigate(['project-list'])
    }
  }

  protected readonly faXmark = faXmark;
}
