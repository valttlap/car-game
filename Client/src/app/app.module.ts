import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SightingComponent } from './views/sighting/sighting.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastContainerComponent } from './toast-container/toast-container.component';

@NgModule({
  declarations: [AppComponent, SightingComponent],
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
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
