import { Component, OnInit } from '@angular/core';
import { SightingClient, SightingUserDto } from 'src/app/services/api';
import { MapService } from 'src/app/services/map.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-sightings-map',
  templateUrl: './sightings-map.component.html',
  styleUrls: ['./sightings-map.component.scss'],
})
export class SightingsMapComponent implements OnInit {
  sightings: SightingUserDto[] = [];

  constructor(
    private sightingClient: SightingClient,
    private toastService: ToastService,
    private mapService: MapService
  ) {}

  ngOnInit(): void {
    this.getSightings();
  }

  private getSightings(): void {
    this.sightingClient.getSightings().subscribe({
      next: sightings => {
        this.sightings = sightings;
        this.mapService.updatePointsFromDatabase(sightings);
      },
      error: error => {
        this.toastService.showError(error);
      },
    });
  }
}
