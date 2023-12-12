import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {TopBarComponent} from "../top-bar/top-bar.component";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [CommonModule, TopBarComponent, RouterLink],
  templateUrl: './error.component.html',
  styleUrl: './error.component.css'
})
export class ErrorComponent {

}
