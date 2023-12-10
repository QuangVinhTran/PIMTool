import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';

import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { HttpClientModule, HttpClient } from '@angular/common/http';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(
    http,

    './assets/i18n/',

    '.json'
  );
}

@NgModule({
  declarations: [],

  imports: [
    CommonModule,

    HttpClientModule,

    TranslateModule.forRoot({
      defaultLanguage: 'en',

      loader: {
        provide: TranslateLoader,

        useFactory: HttpLoaderFactory,

        deps: [HttpClient],
      },
    }),
  ],

  exports: [TranslateModule],
})
export class NgxTranslateModule {}
