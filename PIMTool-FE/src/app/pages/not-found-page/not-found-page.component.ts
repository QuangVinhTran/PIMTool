import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavigationBarComponent} from "../../components/navigation-bar/navigation-bar.component";
import {NotFoundComponent} from "../../components/not-found/not-found.component";

@Component({
  selector: 'app-not-found-page',
  standalone: true,
  imports: [CommonModule, NavigationBarComponent, NotFoundComponent],
  templateUrl: './not-found-page.component.html',
  styleUrl: './not-found-page.component.css'
})
export class NotFoundPageComponent {

}
