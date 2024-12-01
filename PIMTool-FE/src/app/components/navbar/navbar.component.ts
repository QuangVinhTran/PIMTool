import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  showProject: boolean = false;
  showCustomer: boolean = false;
  showSupplier: boolean = false;

  toggleLinks(): void {
    this.showProject = !this.showProject;
    this.showCustomer = !this.showCustomer;
    this.showSupplier = !this.showSupplier;
  }
}
