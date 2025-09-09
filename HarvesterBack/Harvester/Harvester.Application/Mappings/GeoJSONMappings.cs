using Harvester.Application.Dtos;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Mappings
{
    public static class GeoJSONMappings
    {


        public static GeoPointDto MapPointToPointDto(Point point)
        {
            if (point == null) return null;
            return new GeoPointDto
            {
                Coordinates = new double[] { point.Coordinate[0], point.Coordinate[1] }
            };
        }

        public static GeoMultiPolygonDto MapMultiPolygonToDto(MultiPolygon multiPolygon)
        {
            if (multiPolygon == null) return null!;

            var coordinates = new double[multiPolygon.NumGeometries][][][];

            for (int i = 0; i < multiPolygon.NumGeometries; i++)
            {
                var polygon = (Polygon)multiPolygon.GetGeometryN(i);

                var rings = new double[polygon.NumInteriorRings + 1][][];

                rings[0] = polygon.ExteriorRing.Coordinates
                    .Select(c => new double[] { c.X, c.Y })
                    .ToArray();

               
                for (int j = 0; j < polygon.NumInteriorRings; j++)
                {
                    var hole = polygon.GetInteriorRingN(j);
                    rings[j + 1] = hole.Coordinates
                        .Select(c => new double[] { c.X, c.Y })
                        .ToArray();
                }

                coordinates[i] = rings;
            }

            return new GeoMultiPolygonDto
            {
                Coordinates = coordinates
            };
        }

        public static Point MapGeoPointDtoToPoint(GeoPointDto dto)
        {
            if (dto == null) return null;
            return new Point(dto.Coordinates[0], dto.Coordinates[1])
            {
                SRID = 4326
            };
        }
        public static MultiPolygon MapMultiPolygonDtoToMultiPolygon(GeoMultiPolygonDto dto)
        {
            if (dto == null) return null;
            var polygons = dto.Coordinates.Select(poly =>
            {
                var rings = poly.Select(ring =>
                    new LinearRing(ring.Select(p => new Coordinate(p[0], p[1])).ToArray())
                ).ToArray();

                return new Polygon(rings[0], rings.Skip(1).ToArray());
            }).ToArray();

            return new MultiPolygon(polygons) { SRID = 4326 };
        }
    }
}
