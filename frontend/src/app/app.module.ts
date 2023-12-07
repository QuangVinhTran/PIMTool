import {LOCALE_ID, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './layout/footer/footer.component';
import { HeaderComponent } from './layout/header/header.component';
import { DefaultLayoutComponent } from './layout/layout.component';
import { SideBarComponent } from './layout/side-bar/side-bar.component';
import { ProjectListComponent } from './modules/project/project-list/project-list.component';
import { SearchBarComponent } from './modules/project/project-list/search-bar/search-bar.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PaginationComponent } from './modules/project/project-list/pagination/pagination.component';
import { CreateProjectComponent } from './modules/project/create-project/create-project.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { StoreModule } from "@ngrx/store";
import { LoaderComponent } from './core/components/loader/loader.component';
import { LoginComponent } from './modules/login/login.component';
import { PageLoaderComponent } from './core/components/page-loader/page-loader.component';
import { LoginFailedModalComponent } from './core/components/modals/login-failed-modal/login-failed-modal.component';
import { CreateProjectSuccessComponent } from './core/components/modals/create-project-success/create-project-success.component';
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from "@angular/common/http";
import { NotFoundComponent } from './modules/not-found/not-found.component';
import { ErrorComponent } from './modules/error/error.component';
import { DeleteConfirmationComponent } from './core/components/modals/delete-confirmation/delete-confirmation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {CdkDrag, CdkDropList} from "@angular/cdk/drag-drop";
import {DragScrollModule} from "ngx-drag-scroll";
import { DateFormatPipe } from './core/pipes/date-format.pipe';
import { ProjectStatusResolvePipe } from './core/pipes/project-status-resolve.pipe';
import {searchReducer} from "./core/store/search/search.reducer";
import {sortReducer} from "./core/store/sort/sort.reducers";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {routeReducer} from "./core/store/route/route.reducers";
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import {RouteEffects} from "./core/store/route/route.effects";
import { AdvancedFilterComponent } from './modules/project/project-list/advanced-filter/advanced-filter.component';
import {advancedFilterReducer} from "./core/store/advanced-filter/advancedFilter.reducers";
import { DateInputComponent } from './core/components/date-input/date-input.component';
import {NgOptimizedImage} from "@angular/common";
import {HttpClientInterceptor} from "./core/lib/httpClientInterceptor";
import {ToastrModule} from "ngx-toastr";
import { EmployeeFormatPipe } from './core/pipes/employee-format.pipe';
import {MatTooltipModule} from "@angular/material/tooltip";
import {settingReducer} from "./core/store/setting/setting.reducers";
import { FileImportComponent } from './modules/project/components/file-import/file-import.component';
import { FileExportComponent } from './modules/project/components/file-export/file-export.component';
import { pageReducer } from './core/store/page/page.reducer';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    DefaultLayoutComponent,
    SideBarComponent,
    ProjectListComponent,
    SearchBarComponent,
    PaginationComponent,
    CreateProjectComponent,
    LoaderComponent,
    LoginComponent,
    PageLoaderComponent,
    LoginFailedModalComponent,
    CreateProjectSuccessComponent,
    NotFoundComponent,
    ErrorComponent,
    DeleteConfirmationComponent,
    DateFormatPipe,
    ProjectStatusResolvePipe,
    AdvancedFilterComponent,
    DateInputComponent,
    EmployeeFormatPipe,
    FileImportComponent,
    FileExportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    CdkDrag,
    CdkDropList,
    DragScrollModule,
    MatDatepickerModule,
    NgOptimizedImage,
    MatTooltipModule,
    StoreModule.forRoot({
      searchCriteria: searchReducer,
      sortInfo: sortReducer,
      route: routeReducer,
      advancedFilter: advancedFilterReducer,
      settings: settingReducer,
      page: pageReducer
    }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot({
      timeOut: 3000,
      closeButton: true,
      newestOnTop: true,
      progressBar: true,
      progressAnimation: 'decreasing',
      tapToDismiss: false,
    }),
    EffectsModule.forRoot([RouteEffects]),
    StoreRouterConnectingModule.forRoot(),
  ],
  providers: [
    {
      provide: LOCALE_ID,
      useValue: 'en-US'
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpClientInterceptor,
      multi: true
    }
  ],
  exports: [
    ProjectStatusResolvePipe,
    DateFormatPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http)
}
