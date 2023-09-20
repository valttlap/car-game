import {
  SightingClient,
  PlateClient,
  PlateDto,
  SightingDto,
} from './../../services/api';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, map, of, startWith, switchMap, take } from 'rxjs';
import { ConvertToGeoJSON } from 'src/app/helpers/convert-to-geojson';
import { ToastService } from 'src/app/services/toast.service';
@Component({
  selector: 'app-sighting',
  templateUrl: './sighting.component.html',
  styleUrls: ['./sighting.component.scss'],
})
export class SightingComponent implements OnInit {
  private _currLocation?: GeolocationPosition;
  private _plateDto?: PlateDto;

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
    diplomatNumber: new FormControl<number | null>(null),
    description: new FormControl<string | null>(null),
  });

  plates$: Observable<PlateDto[]>;

  constructor(
    private SightingClient: SightingClient,
    private plateClient: PlateClient,
    private toastService: ToastService,
    public auth: AuthService
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

  get currLocation(): GeolocationPosition | undefined {
    return this._currLocation;
  }

  set currLocation(value: GeolocationPosition) {
    this._currLocation = value;
  }

  get plateDto(): PlateDto | undefined {
    return this._plateDto;
  }

  set plateDto(value: PlateDto) {
    this._plateDto = value;
  }

  onPlateChange(event: Event) {
    const target = event.target as HTMLInputElement;
    const selectedValue = target.value;
    this.plates$
      .pipe(
        take(1),
        map(plates =>
          plates.find(plate =>
            plate.isDiplomat
              ? plate.diplomatCode === parseInt(selectedValue)
              : plate.countryAbbreviation === selectedValue
          )
        )
      )
      .subscribe({
        next: selectedPlate => {
          if (selectedPlate) {
            this.plateDto = selectedPlate;
          }
        },
        error: error => {
          this.toastService.showError(error);
        },
      });
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
        (error: GeolocationPositionError) =>
          this.toastService.showError(error.message),
        this.positionOptions
      );
    } else {
      this.toastService.showError('No support for geolocation');
    }
  }

  onSubmit(): void {
    try {
      const sighting: SightingDto = {
        plateId:
          this.plateDto?.id ??
          (() => {
            throw new Error('Plate id found');
          })(),
        isDiplomat: this.sightingForm.get('isDiplomat')?.value ?? false,
        description: this.sightingForm.get('description')?.value ?? undefined,
        location: this.currLocation?.coords
          ? ConvertToGeoJSON(this.currLocation?.coords)
          : (() => {
            throw new Error('No location found');
          })(),
        srid: 4326,
        date: new Date(),
      };
      this.SightingClient.addSighting(sighting).subscribe({
        next: (sighting: SightingDto) => {
          this.toastService.showSuccess('Havainto lisätty!');
        },
        error: error => {
          this.toastService.showError(error);
        },
      });
    } catch (error) {
      if (error instanceof Error) this.toastService.showError(error.message);
      return;
    }
  }
}
