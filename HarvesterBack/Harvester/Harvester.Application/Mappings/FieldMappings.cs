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
                Name = field.IdentifierName,
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
    }
}
