import { AfterViewInit, Component, ElementRef, OnInit } from '@angular/core';
import { MapService } from '../services/map.service';

@Component({
  selector: 'app-map-view',
  template: '',
  styles: [
    `
      :host {
        display: block;
        width: 100%;
        height: 400px; // Adjust as needed
      }
    `,
  ],
  standalone: true,
})
export class MapViewComponent implements OnInit, AfterViewInit {
  constructor(
    private mapService: MapService,
    private el: ElementRef
  ) {}

  ngOnInit(): void {
    this.mapService.initializeMap(this.el.nativeElement.id);
  }

  ngAfterViewInit(): void {
    const map = this.mapService.getMap();
    if (!map) {
      throw new Error('Map not initialized');
    }
    map.updateSize();
  }
}
