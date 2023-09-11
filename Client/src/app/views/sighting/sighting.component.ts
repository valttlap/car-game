import { SightingClient, PlateClient, PlateDto } from './../../services/api';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, of, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-sighting',
  templateUrl: './sighting.component.html',
  styleUrls: ['./sighting.component.scss'],
})
export class SightingComponent implements OnInit {
  private _currLocation?: GeolocationPosition;

  watcherId?: number;
  // navigator.geolocation.clearWatch(this.watcherId);

  positionOptions: PositionOptions = {
    enableHighAccuracy: true,
    timeout: 10000,
    maximumAge: 0,
  };

  sightingForm = new FormGroup({
    isDiplomat: new FormControl<boolean>(false),
    plateId: new FormControl<string>(''),
    diplomatNumber: new FormControl<number>(1),
    description: new FormControl<string>(''),
    srid: new FormControl<number>(4326),
  });

  plates$: Observable<PlateDto[]>;

  constructor(
    private SightingClient: SightingClient,
    private plateClient: PlateClient
  ) {
    type PlateIdValue = string | PlateDto | null;

    this.plates$ =
      this.sightingForm.get('plateId')?.valueChanges.pipe(
        startWith(''),
        switchMap((value: PlateIdValue) =>
          this.plateClient.findPlatesByAbbr(
            typeof value === 'string' ? value : value?.countryAbbreviation,
            this.sightingForm.get('isDiplomat')?.value ?? false
          )
        )
      ) ?? of([]);
  }

  displayFn(plate: PlateDto): string {
    return plate
      ? plate.isDiplomat
        ? plate.diplomatCode?.toString() ?? ''
        : plate.countryAbbreviation ?? ''
      : '';
  }

  get currLocation(): GeolocationPosition | undefined {
    return this._currLocation;
  }

  set currLocation(value: GeolocationPosition) {
    this._currLocation = value;
  }

  ngOnInit(): void {
    this.getLocation();
  }

  getLocation(): void {
    if (navigator.geolocation) {
      this.watcherId = navigator.geolocation.watchPosition(
        (position: GeolocationPosition) => {
          this.currLocation = position;
        },
        (error: GeolocationPositionError) => console.error(error),
        this.positionOptions
      );
    } else {
      console.log('No support for geolocation');
    }
  }
}
