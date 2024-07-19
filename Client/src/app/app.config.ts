import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AuthModule, AuthHttpInterceptor } from '@auth0/auth0-angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { API_BASE_URL } from './services/api';
import { environment as env } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    importProvidersFrom(
      BrowserModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      MatAutocompleteModule,
      MatOptionModule,
      AuthModule.forRoot({
        ...env.auth0,
        cacheLocation: 'localstorage',
        httpInterceptor: {
          allowedList: ['*'],
        },
      })
    ),
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: API_BASE_URL, useValue: env.api.apiUrl },
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimations(),
  ],
};
