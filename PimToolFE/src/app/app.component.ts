import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import {TopBarComponent} from "./core/components/top-bar/top-bar.component";
import {BodyComponent} from "./core/components/body/body.component";
import {HttpClient} from "@angular/common/http";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, TopBarComponent, BodyComponent, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'PimToolFE';
}
