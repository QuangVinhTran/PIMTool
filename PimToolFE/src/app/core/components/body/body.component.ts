import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LeftBodyComponent} from "./left-body/left-body.component";
import {RightBodyComponent} from "./right-body/right-body.component";
import {RouterOutlet} from "@angular/router";

@Component({
  selector: 'app-body',
  standalone: true,
  imports: [CommonModule, LeftBodyComponent, RightBodyComponent, RightBodyComponent, RouterOutlet],
  templateUrl: './body.component.html',
  styleUrl: './body.component.css'
})
export class BodyComponent {

}