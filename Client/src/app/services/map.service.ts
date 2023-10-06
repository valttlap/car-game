import { Injectable } from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import VectorLayer from 'ol/layer/Vector';
import VectorSource from 'ol/source/Vector';
import Point from 'ol/geom/Point';
import Feature from 'ol/Feature';
import { Circle as CircleStyle, Fill, Icon, Stroke, Style } from 'ol/style';
import { Circle, Geometry } from 'ol/geom';
import GeoJSON from 'ol/format/GeoJSON';
import { transform } from 'ol/proj';
import { SightingUserDto } from './api';
import proj4 from 'proj4';
import { register } from 'ol/proj/proj4';

@Injectable({
  providedIn: 'root',
})
export class MapService {
  private map?: Map;
  private userPositionLayer?: VectorLayer<VectorSource<Geometry>>;
  private pointsLayer?: VectorLayer<VectorSource<Geometry>>;

  initializeMap(targetId: string): void {
    const userPositionSource = new VectorSource();
    this.userPositionLayer = new VectorLayer({
      source: userPositionSource,
      style: new Style({
        image: new CircleStyle({
          radius: 6,
          fill: new Fill({ color: 'red' }),
          stroke: new Stroke({ color: 'white', width: 2 }),
        }),
      }),
    });

    const pointSource = new VectorSource();
    this.pointsLayer = new VectorLayer({
      source: pointSource,
    });

    this.map = new Map({
      target: targetId,
      layers: [
        new TileLayer({
          source: new OSM(),
        }),
        this.userPositionLayer,
        this.pointsLayer,
      ],
      view: new View({
        center: [0, 0],
        zoom: 2,
      }),
    });
  }

  updateUserPosition(coords: [number, number], accuracy?: number): void {
    const transformedCoords = transform(coords, 'EPSG:4326', 'EPSG:3857');
    const feature = new Feature({
      geometry: new Point(transformedCoords),
      type: 'position',
    });

    const featuresToAdd: Feature[] = [feature]; // Explicitly type as Feature[]

    if (accuracy) {
      const accuracyFeature = new Feature({
        geometry: new Circle(transformedCoords, accuracy),
        type: 'accuracy',
      });
      featuresToAdd.push(accuracyFeature);
    }

    this.userPositionLayer?.getSource()?.clear();
    this.userPositionLayer?.getSource()?.addFeatures(featuresToAdd);

    // Optional: Style the accuracy feature differently
    this.userPositionLayer?.setStyle(feature => {
      if (feature.get('type') === 'position') {
        return new Style({
          image: new CircleStyle({
            radius: 6,
            fill: new Fill({ color: 'red' }),
            stroke: new Stroke({ color: 'white', width: 2 }),
          }),
        });
      } else if (feature.get('type') === 'accuracy') {
        return new Style({
          fill: new Fill({ color: 'rgba(255, 0, 0, 0.3)' }), // semi-transparent red fill
        });
      } else {
        // Default style
        return new Style({
          fill: new Fill({ color: 'rgba(0, 0, 0, 0.3)' }), // semi-transparent black fill as default
        });
      }
    });
    this.map?.getView().setCenter(transformedCoords);
    this.map?.getView().setZoom(15);
  }

  updatePointsFromDatabase(data: SightingUserDto[]) {
    proj4.defs(
      'EPSG:3067',
      '+proj=utm +zone=35 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs +type=crs'
    );
    register(proj4);
    const geoJsonFormat = new GeoJSON();
    const features: Feature[] = [];

    data.forEach(item => {
      // Parse the location string to get the GeoJSON object
      const geoJsonObj = JSON.parse(item.location);

      // Read the feature from the GeoJSON object
      const feature = geoJsonFormat.readFeature(geoJsonObj);
      // Transform the coordinates from the given srid to 'EPSG:3857'
      feature.getGeometry()?.transform(`EPSG:${item.srid}`, 'EPSG:3857');

      feature.setProperties({
        country: item.country,
        description: item.description,
        date: item.date,
        isDiplomat: item.isDiplomat,
        diplomatNumber: item.diplomatNumber,
      });

      features.push(feature);
    });

    // Clear the existing features and add the new ones
    this.pointsLayer?.getSource()?.clear();
    this.pointsLayer?.getSource()?.addFeatures(features);

    // Update the style function for the pointsLayer
    this.pointsLayer?.setStyle(() => {
      return new Style({
        image: new CircleStyle({
          radius: 6,
          fill: new Fill({ color: 'red' }),
          stroke: new Stroke({ color: 'white', width: 2 }),
        }),
      });
    });
  }

  getMap(): Map | undefined {
    return this.map;
  }

  deleteAll(): void {
    this.pointsLayer?.getSource()?.clear();
    this.userPositionLayer?.getSource()?.clear();
  }
}
