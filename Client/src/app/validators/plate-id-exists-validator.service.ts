import { PlateClient } from './../services/api';
import { Observable, catchError, map, of } from 'rxjs';
import { Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class PlateIdExistsValidatorService implements AsyncValidator {
  constructor(private plateClient: PlateClient) {}

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.plateClient.getPlateById(control.value).pipe(
      map(plate => (plate ? { plateNotExists: true } : null)),
      catchError(() => of({ plateNotExists: true }))
    );
  }
}
