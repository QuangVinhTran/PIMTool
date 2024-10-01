import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { ApiConfiguration } from './swagger/api-configuration';

/**
 * Global configuration for Api services
 */
@Injectable({
  providedIn: 'root',
})
export class EnvironmentApiConfiguration extends ApiConfiguration {
  rootUrl = environment.apiUrl;
}
