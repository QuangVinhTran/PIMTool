import { Location } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppRoutingModule } from './app-routing.module';
import { PIMBaseModule } from './Base/base.module';
import { ShellComponent } from './Shell/components';
import { ShellModule } from './Shell/shell.module';
import { ApiConfiguration } from './swagger/api-configuration';
import { EnvironmentApiConfiguration } from './api-config';

export function HttpLoaderFactory(http: HttpClient, loc: Location) {
  return new TranslateHttpLoader(http, loc.prepareExternalUrl('/assets/i18n/'), '.json');
}

@NgModule({
  declarations: [
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PIMBaseModule.forRoot(),
    ShellModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient, Location]
      }
    }),
    HttpClientModule
  ],
  providers: [
    {
      provide: ApiConfiguration,
      useClass: EnvironmentApiConfiguration as any
    }
  ],
  bootstrap: [ShellComponent]
})
export class AppModule {
  constructor(translate: TranslateService) {
    translate.setDefaultLang('en');
  }
}
