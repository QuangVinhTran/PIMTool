import { LoginRequest } from './../models/login-request.model';
import { Component, OnDestroy } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { LoginResponse } from '../models/login-response.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [],
})
export class LoginComponent implements OnDestroy {
  // model: LoginRequest;
  private loginSubscription?: Subscription;
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['Admin', Validators.required],
      password: ['1', Validators.required],
    });
  }

  get username() {
    return this.loginForm.controls['username'];
  }

  get password() {
    return this.loginForm.controls['password'];
  }
  onFormSubmit() {
    if (this.loginForm.valid) {
      const postData = { ...this.loginForm.value };
      this.loginSubscription = this.authService
        .Login(postData as LoginRequest)
        .subscribe({
          next: (response: LoginResponse) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: response.message,
            });

            this.router.navigateByUrl('/home');
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: err.error.message,
            });
          },
        });
    } else {
      console.log('Form is invalid');
      // Optionally, display a message to the user or perform other actions for an invalid form
    }
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
  }
}
