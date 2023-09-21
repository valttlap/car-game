import {
  Component,
  OnInit,
  OnChanges,
  SimpleChanges,
  Input,
  AfterViewInit,
} from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import Feature from 'ol/Feature';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { fromLonLat } from 'ol/proj';
import Polygon from 'ol/geom/Polygon';
import Point from 'ol/geom/Point';
import { Vector as VectorSource } from 'ol/source';
import { Vector as VectorLayer } from 'ol/layer';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
})
export class MapComponent implements AfterViewInit, OnChanges {
  @Input() currLocation?: GeolocationPosition;
  map!: Map;
  vectorSource!: VectorSource;

  ngAfterViewInit(): void {
    this.vectorSource = new VectorSource();

    this.map = new Map({
      target: 'map',
      layers: [
        new TileLayer({
          source: new OSM(),
        }),
        new VectorLayer({
          source: this.vectorSource,
        }),
      ],
      view: new View({
        center: [0, 0],
        zoom: 2,
      }),
    });
    this.updateLocation(this.currLocation);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.map) return;
    if (changes['currLocation'] && changes['currLocation'].currentValue) {
      this.updateLocation(this.currLocation);
    }
  }

  updateLocation(position: GeolocationPosition | undefined): void {
    if (!position) {
      return;
    }
    const coords = fromLonLat([
      position.coords.longitude,
      position.coords.latitude,
    ]);
    this.map.getView().animate({ center: coords, zoom: 12 });

    // Create a circular polygon to represent the accuracy
    const accuracyFeature = new Feature(
      new Polygon([
        Array.from({ length: 64 }, (_, i) => [
          position.coords.longitude +
            position.coords.accuracy * Math.cos((2 * Math.PI * i) / 64),
          position.coords.latitude +
            position.coords.accuracy * Math.sin((2 * Math.PI * i) / 64),
        ]).map(coord => fromLonLat(coord)),
      ])
    );

    const positionFeature = new Feature(new Point(coords));
    positionFeature.setStyle(
      new Style({
        image: new CircleStyle({
          radius: 6,
          fill: new Fill({
            color: '#3399CC',
          }),
          stroke: new Stroke({
            color: '#fff',
            width: 2,
          }),
        }),
      })
    );

    this.vectorSource.clear(true);
    this.vectorSource.addFeatures([accuracyFeature, positionFeature]);
  }
}
