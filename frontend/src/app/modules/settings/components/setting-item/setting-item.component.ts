import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-setting-item',
  templateUrl: './setting-item.component.html',
  styleUrls: ['./setting-item.component.scss']
})
export class SettingItemComponent {
  @Input() text?: string
  @Input() status?: boolean
  @Output() toggle: EventEmitter<void> = new EventEmitter<void>()

  handleToggle() {
    this.toggle.emit()
  }
}
