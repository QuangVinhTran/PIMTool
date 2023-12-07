import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import {ToastrModule} from "ngx-toastr";

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideAnimations()]
};
