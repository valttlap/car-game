import {
  SightingClient,
  PlateClient,
  PlateDto,
  SightingDto,
  SightingUserDto,
} from './../../services/api';
import {
  Component,
  OnInit,
  QueryList,
  TemplateRef,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from '@auth0/auth0-angular';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, map, of, startWith, switchMap, take } from 'rxjs';
import {
  SortEvent,
  SortHeaderDirective,
} from 'src/app/dirctives/sort-header.directive';
import { ConvertToGeoJSON } from 'src/app/helpers/convert-to-geojson';
import { ToastService } from 'src/app/services/toast.service';

const compare = (v1: string | Date, v2: string | Date) =>
  v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

@Component({
  selector: 'app-sighting',
  templateUrl: './sighting.component.html',
  styleUrls: ['./sighting.component.scss'],
})
export class SightingComponent implements OnInit {
  private _currLocation?: GeolocationPosition;
  private _plateDto?: PlateDto;

  active = 1;

  sightings: SightingUserDto[] = [];

  @ViewChildren(SortHeaderDirective)
  headers?: QueryList<SortHeaderDirective>;

  @ViewChild('content') content?: TemplateRef<unknown>;

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
    public auth: AuthService,
    private modalService: NgbModal
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
    this.getSightings();
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

  getSightings(): void {
    this.SightingClient.getSightings().subscribe({
      next: sightings => {
        this.sightings = sightings;
      },
      error: error => {
        this.toastService.showError(error);
      },
    });
  }

  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    this.headers?.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting countries
    if (direction === '' || column === '') {
      this.getSightings();
    } else {
      let res: 1 | 0 | -1;
      this.sightings = [...this.sightings].sort((a, b) => {
        switch (column) {
          case 'country':
            res = compare(a.country, b.country);
            return direction === 'asc' ? res : -res;
          case 'date':
            res = compare(a.date, b.date);
            return direction === 'asc' ? res : -res;
          default:
            return 0;
        }
      });
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
        next: () => {
          this.toastService.showSuccess('Havainto lisätty!');
          this.sightingForm.reset();
          this.getSightings();
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

  deleteSighting(id: number): void {
    this.modalService
      .open(this.content, {
        ariaLabelledBy: 'modal-basic-title',
      })
      .result.then(result => {
        if (result) {
          this.SightingClient.deleteSighting(id).subscribe({
            next: () => {
              this.toastService.showSuccess('Havainto poistettu!');
              this.getSightings();
            },
            error: error => {
              this.toastService.showError(error);
            },
          });
        }
      });
  }
}
