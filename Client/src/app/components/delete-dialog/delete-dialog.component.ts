import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-delete-dialog',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.scss',
})
export class DeleteDialogComponent {
  constructor(public ref: DynamicDialogRef) {}

  cancel(): void {
    this.ref.close(false);
  }

  confirmDelete(): void {
    this.ref.close(true);
  }
}
