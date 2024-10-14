import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-test',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent {
  @Input() show: boolean = false;
  @Output() showChange : EventEmitter<boolean> = new EventEmitter<boolean>();

  @Output() result = new EventEmitter<void>();
  handleAccept() {
    this.show = false;
    this.showChange.emit(this.show);
    this.result.emit();
  }
  cancel() {
    this.show = false;
    this.showChange.emit(this.show);
  }
}
