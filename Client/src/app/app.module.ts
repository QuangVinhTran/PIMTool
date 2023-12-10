import { NgModule, ViewChild } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './features/public/home/home.component';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButtonModule } from 'primeng/button';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HeaderComponent } from './components/header/header.component';
import { ProjectListComponent } from './features/project/project-list/project-list.component';
import { CreateProjectComponent } from './features/project/add-edit-project/add-edit-project.component';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { DialogModule } from 'primeng/dialog';
import { PaginatorModule } from 'primeng/paginator';
import { DatePipe } from '@angular/common';
import { CalendarModule } from 'primeng/calendar';
import { StoreModule } from '@ngrx/store';
import { globalStateReducer } from './SearchValueRx/global-state.reducer';

@NgModule({
  declarations: [AppComponent, LoginComponent, HomeComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastModule,
    ButtonModule,
    DropdownModule,
    MultiSelectModule,
    HeaderComponent,
    SidebarComponent,
    ProjectListComponent,
    CreateProjectComponent,
    DialogModule,
    PaginatorModule,
    CalendarModule,
    StoreModule.forRoot({ globalState: globalStateReducer }),
  ],
  providers: [MessageService, DatePipe],
  bootstrap: [AppComponent],
})
export class AppModule {}
