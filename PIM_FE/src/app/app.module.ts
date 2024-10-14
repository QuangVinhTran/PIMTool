import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layout/header/header.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { LayoutComponent } from './layout/layout.component';
import { ProjectListComponent } from './project/project-list/project-list.component';
import { ProjectNewComponent } from './project/project-new/project-new.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  HttpClientModule,
} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ConfirmComponent } from './confirm/confirm.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LoginComponent } from './login/login.component';
import { AuthInterceptor } from './guard/authInterceptor';
import { StoreModule } from '@ngrx/store';
import { metaReducerLocalStorage, searchSortReducer } from './search-sort-state/search-sort.reducer';

// export function HttpLeaderFactory(http : HttpClient) {
//   return new TranslateHttpLoader(http);
// }

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    NavbarComponent,
    LayoutComponent,
    ProjectListComponent,
    ErrorPageComponent,
    ConfirmComponent,
    LoginComponent,
    // ProjectNewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule.forRoot(),
    StoreModule.forRoot({ searchSortEntries: searchSortReducer }),
    // StoreModule.forRoot({ cartEntries: cartReducer }, { metaReducers: [ metaReducerLocalStorage ] })
    MatTooltipModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLeaderFactory,
        deps: [HttpClient],
      },
    }),
    StoreModule.forRoot({}, {}),
  ],
  providers: [
    HttpClient,
    DatePipe,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

export function HttpLeaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
