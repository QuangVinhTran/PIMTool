import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ResponseDto } from 'src/shared/models/responseDto';
import { AuthService } from 'src/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  errorMessage : string = '';
  constructor(private formBuilder: FormBuilder, private authService : AuthService, private router : Router) {}

  loginForm = this.formBuilder.group({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });
  public login() {
    if(this.loginForm.valid) {
      if(this.loginForm.controls.username) {
        const username =  this.loginForm.get('username')?.value?.trim();
        const password =  this.loginForm.get('password')?.value?.trim();
        
        this.authService.login(username!, password!).subscribe((res: ResponseDto) => {
          if(res.isSuccess) {
            localStorage.setItem('access_token', res.data);
            this.router.navigate(['project-list']);
          } else {
            this.errorMessage = res.error;
          }
        }
        )
      }
    } else {
      this.errorMessage = 'pleaseInputLogin'
    }
  }
}
