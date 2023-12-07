import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink, RouterLinkActive} from "@angular/router";

@Component({
  selector: 'app-left-body',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './left-body.component.html',
  styleUrl: './left-body.component.css'
})
export class LeftBodyComponent {

}
