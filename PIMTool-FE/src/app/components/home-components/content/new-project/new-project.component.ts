import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormsModule} from '@angular/forms';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatInputModule} from "@angular/material/input";
import {MatChipEditedEvent, MatChipInputEvent, MatChipsModule} from '@angular/material/chips';
import {LiveAnnouncer} from '@angular/cdk/a11y';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatAutocompleteModule} from '@angular/material/autocomplete';


export interface Fruit {
  name: string;
}

export interface Option {
  id: number,
  name: string
}

@Component({
  selector: 'app-new-project',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatSelectModule, MatFormFieldModule, FormsModule, MatChipsModule, MatIconModule,
          MatAutocompleteModule],
  templateUrl: './new-project.component.html',
  styleUrl: './new-project.component.css'
})
export class NewProjectComponent {
  addOnBlur = true;
  readonly separatorKeysCodes = [ENTER, COMMA] as const;
  fruits: Fruit[] = [];
  options: Option[] = [
    {id: 1, name: "Nguyên"},
    {id: 2, name: "Minh"}
  ];
  input: string = "";

  announcer = inject(LiveAnnouncer);

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our fruit
    if (value) {
      this.fruits.push({name: value});
      console.log(this.fruits);
    }

    // Clear the input value
    event.chipInput!.clear();
  }

  remove(fruit: Fruit): void {
    const index = this.fruits.indexOf(fruit);

    if (index >= 0) {
      this.fruits.splice(index, 1);
      this.announcer.announce(`Removed ${fruit}`);
    }
  }

  edit(fruit: Fruit, event: MatChipEditedEvent) {
    const value = event.value.trim();

    // Remove fruit if it no longer has a name
    if (!value) {
      this.remove(fruit);
      return;
    }

    // Edit existing fruit
    const index = this.fruits.indexOf(fruit);
    if (index >= 0) {
      this.fruits[index].name = value;
    }
  }
}
