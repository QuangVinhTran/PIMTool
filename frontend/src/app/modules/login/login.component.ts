import { Component, OnInit, OnDestroy } from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import BASE_URL, {EndPoints} from "../../data/apiInfo";
import {HttpClient} from "@angular/common/http";
import {ApiResponse} from "../../core/services/project.service";
import {routes} from "../../core/constants/routeConstants";
import {Router} from "@angular/router";
import {jwtDecode} from "jwt-decode";
import { TokenTypes } from 'src/app/core/constants/tokenConstants';
import { Store } from '@ngrx/store';
import { setLoadingOff, setLoadingOn } from 'src/app/core/store/page/page.actions';
import { Subscription } from 'rxjs';
import { selectLoadingState } from 'src/app/core/store/page/page.selectors';
import { SubscriptionService } from 'src/app/core/services/subscription.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private store: Store,
    private subService: SubscriptionService
  ) {}

  isValidEmail = true
  isValidPassword = true
  isLoading = false
  isLoginSuccess = true
  errorMsg = ''
  subscriptions: Subscription[] =[]

  loginForm = this.formBuilder.group({
    email: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')
    ]),
    password: new FormControl('', [
      Validators.required,
      // Validators.pattern('^(?=.*[A-Z])(?=.*[!@#$%^&*()-_+=]).{8,}$')
    ])
  })

  ngOnInit(): void {
    this.subscriptions.push(
      this.store.select(selectLoadingState).subscribe(value => this.isLoading = value)
    )
  }

  ngOnDestroy(): void {
    this.subService.unsubscribe(this.subscriptions)
  }

  validateEmail() {
    this.isValidEmail = !this.loginForm.get('email')?.invalid && !this.loginForm.pristine
  }

  validatePassword() {
    this.isValidPassword = !this.loginForm.get('password')?.invalid && !this.loginForm.pristine
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.validateEmail()
      this.validatePassword()
      return
    }

    this.store.dispatch(setLoadingOn())
    this.http.post<ApiResponse>(
      `${BASE_URL}/${EndPoints.LOGIN}`,
      this.loginForm.getRawValue()
    ).subscribe({
      next: (response) =>  {
        if (response.isSuccess) {
          localStorage.setItem(TokenTypes.ACCESS_TOKEN, response.data?.accessToken)
          localStorage.setItem(TokenTypes.REFRESH_TOKEN, response.data?.refreshToken)
          this.store.dispatch(setLoadingOff())
          this.router.navigate([routes.PROJECT_LIST])
          const expireTime = jwtDecode(response.data?.accessToken).exp
          expireTime && setTimeout(() => {
            console.log('request for refresh token')
          }, expireTime - (Date.now() + 60 * 1000))
        } else {
          this.isLoginSuccess = false
          this.store.dispatch(setLoadingOff())
        }
      }
    })
  }
}
