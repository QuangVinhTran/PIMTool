import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.scss']
})
export class DateInputComponent {
  @Input() date!: string
  @Output() dateChange = new EventEmitter<string>()

  onDateChange(newDate: string) {
    this.date = newDate
    this.dateChange.emit(newDate)
  }

}
