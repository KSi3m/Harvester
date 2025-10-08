using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using NetTopologySuite.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Mappings
{
    public static class FieldMappings
    {
        public static FieldDto MapFieldtoFieldDto(Field field)
        {
            var fieldDto = new FieldDto
            {
                Id = field.Id,
                IdentifierName = field.IdentifierName,
                CommonName = field.CommonName,
                AreaHectares = field.AreaHectares,
                TerrainCoeff = field.TerrainCoeff,
                ShapeCoeff = field.ShapeCoeff,
                CropType = field.CropType,
                Boundary = GeoJSONMappings.MapMultiPolygonToDto(field.Boundary),
                CenterPoint = GeoJSONMappings.MapPointToPointDto(field.CenterPoint)
            };
            return fieldDto;
        }
        public static IEnumerable<FieldDto> MapFieldsToFieldDtos(IEnumerable<Field> fields)
        {
            return fields.Select(MapFieldtoFieldDto);
        }

        public static Field MapCreateFieldDtoToField(CreateFieldDto dto)
        {
            var field = new Field()
            {
                IdentifierName = dto.IdentifierName,
                CommonName = dto.CommonName,
                AreaHectares = dto.AreaHectares,
                TerrainCoeff = dto.TerrainCoeff ?? 1,
                ShapeCoeff = dto.ShapeCoeff ?? 1,
                CropType = dto.CropType,
                CenterPoint = GeoJSONMappings.MapGeoPointDtoToPoint(dto.CenterPoint),
                Boundary = GeoJSONMappings.MapMultiPolygonDtoToMultiPolygon(dto.Boundary),
            };
            return field;
        }
    }
}
