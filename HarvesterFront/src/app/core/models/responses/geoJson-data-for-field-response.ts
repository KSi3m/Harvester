import { GeoMultiPolygonDto } from '../geoJson/geo-multiPolygon-dto';
import { GeoPointDto } from '../geoJson/geo-point-dto';

export interface GeoJsonDataForFieldResponse {
  areaHectares: number;
  centerPoint: GeoPointDto;
  boundary: GeoMultiPolygonDto;
}
