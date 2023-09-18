export const ConvertToGeoJSON = (coords: GeolocationCoordinates) => {
  return JSON.stringify({
    type: 'Point',
    coordinates: [coords.longitude, coords.latitude],
  });
};
