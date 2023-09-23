import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastContainerComponent } from './toast-container/toast-container.component';
import { environment as env } from '../environments/environment';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { API_BASE_URL } from './services/api';
import { SortHeaderDirective } from './dirctives/sort-header.directive';
import { MapViewComponent } from './map-view/map-view.component';
import { AddSightingComponent } from './views/add-sighting/add-sighting.component';
import { ListSightingsComponent } from './views/list-sightings/list-sightings.component';
import { MainComponent } from './main/main.component';

@NgModule({
  declarations: [
    AppComponent,
    SortHeaderDirective,
    MapViewComponent,
    AddSightingComponent,
    ListSightingsComponent,
    MainComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatOptionModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastContainerComponent,
    AuthModule.forRoot({
      ...env.auth0,
      cacheLocation: 'localstorage',
      httpInterceptor: {
        allowedList: ['*'],
      },
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: API_BASE_URL, useValue: env.api.apiUrl },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
