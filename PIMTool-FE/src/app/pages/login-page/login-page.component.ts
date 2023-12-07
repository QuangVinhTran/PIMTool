import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavigationBarComponent} from "../../components/navigation-bar/navigation-bar.component";
import {LoginComponent} from "../../components/login/login.component";

@Component({
  imports: [CommonModule, NavigationBarComponent, LoginComponent],
  selector: 'app-login-page',
  standalone: true,
  styleUrl: './login-page.component.css',
  templateUrl: './login-page.component.html'
})
export class LoginPageComponent {

}
