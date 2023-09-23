import { PlateClient, SightingClient, SightingDto } from './../../services/api';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, map, of, startWith, switchMap, take } from 'rxjs';
import { ConvertToGeoJSON } from 'src/app/helpers/convert-to-geojson';
import { PlateDto } from 'src/app/services/api';
import { MapService } from 'src/app/services/map.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-add-sighting',
  templateUrl: './add-sighting.component.html',
  styleUrls: ['./add-sighting.component.scss'],
})
export class AddSightingComponent implements OnInit, OnDestroy {
  private _plateDto?: PlateDto;

  watcherId?: number;

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

  currentPosition?: GeolocationPosition;

  get plateDto(): PlateDto | undefined {
    return this._plateDto;
  }

  set plateDto(value: PlateDto) {
    this._plateDto = value;
  }

  constructor(
    private plateClient: PlateClient,
    private sightingClient: SightingClient,
    private mapService: MapService,
    private toastService: ToastService
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
  ngOnDestroy(): void {
    this.watcherId && navigator.geolocation.clearWatch(this.watcherId);
  }

  ngOnInit(): void {
    this.initLocation();
  }

  private initLocation(): void {
    if (navigator.geolocation) {
      this.watcherId = navigator.geolocation.watchPosition(
        (position: GeolocationPosition) => {
          const coords: [number, number] = [
            position.coords.longitude,
            position.coords.latitude,
          ];
          const accuracy = position.coords.accuracy;
          this.currentPosition = position;
          this.mapService.updateUserPosition(coords, accuracy);
        },
        (error: GeolocationPositionError) =>
          this.toastService.showError(error.message),
        this.positionOptions
      );
    } else {
      this.toastService.showError('No support for geolocation');
    }
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
        location: this.currentPosition?.coords
          ? ConvertToGeoJSON(this.currentPosition?.coords)
          : (() => {
              throw new Error('No location found');
            })(),
        srid: 4326,
        date: new Date(),
      };
      this.sightingClient.addSighting(sighting).subscribe({
        next: () => {
          this.toastService.showSuccess('Havainto lisätty!');
          this.sightingForm.reset();
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
