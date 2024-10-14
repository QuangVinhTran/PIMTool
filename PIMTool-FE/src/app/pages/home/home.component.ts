import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavigationBarComponent} from "../../components/navigation-bar/navigation-bar.component";
import {SidebarComponent} from "../../components/home-components/sidebar/sidebar.component";
import {ContentComponent} from "../../components/home-components/content/content.component";
import {FooterComponent} from "../../components/footer/footer.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NavigationBarComponent, SidebarComponent, ContentComponent, FooterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
