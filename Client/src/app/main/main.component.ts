import { Component } from '@angular/core';
import { SightingsMapComponent } from '../views/sightings-map/sightings-map.component';
import { ListSightingsComponent } from '../views/list-sightings/list-sightings.component';
import { AddSightingComponent } from '../views/add-sighting/add-sighting.component';
import {
  NgbNav,
  NgbNavItem,
  NgbNavItemRole,
  NgbNavLinkButton,
  NgbNavLinkBase,
  NgbNavContent,
  NgbNavOutlet,
} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  standalone: true,
  imports: [
    NgbNav,
    NgbNavItem,
    NgbNavItemRole,
    NgbNavLinkButton,
    NgbNavLinkBase,
    NgbNavContent,
    AddSightingComponent,
    ListSightingsComponent,
    SightingsMapComponent,
    NgbNavOutlet,
  ],
})
export class MainComponent {
  active = 1;
}
