import { GeoMultiPolygonDto } from './geoJson/geo-multiPolygon-dto';
import { GeoPointDto } from './geoJson/geo-point-dto';

export interface Field {
  id: number;
  identifierName: string;
  commonName: string;
  areaHectares: number;
  terrainCoeff: number;
  shapeCoeff: number;
  cropType: string;
  centerPoint: GeoPointDto;
  boundary: GeoMultiPolygonDto;
}
