import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AuthenticationService} from "../../service/authentication.service";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {LocalStorageService} from "../../service/local-storage.service";
import {ToastrModule, ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, ToastrModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [HttpClient, AuthenticationService, LocalStorageService, ToastrService]
})
export class LoginComponent implements OnInit{
  errorMessage: string = "";
  loginForm: FormGroup = new FormGroup<any>({});

  constructor(private router: Router,
              private authenticationService: AuthenticationService,
              private localStorageService: LocalStorageService) {
  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }

  onSubmitLogin() {
    this.authenticationService.authentication(this.loginForm.value).subscribe(response => {
      this.localStorageService.passingDataToLocal("token", response.body.data.token);
      console.log(response.body.data.token);
      this.router.navigateByUrl("/");
    }, error => {
      this.errorMessage = "Wrong username or password!";
    })
  }

}
