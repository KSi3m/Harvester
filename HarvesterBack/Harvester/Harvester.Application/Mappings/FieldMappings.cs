using Harvester.Domain.Models;
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
                Name = field.Name,
                AreaHectares = field.AreaHectares,
                TerrainCoeff = field.TerrainCoeff,
                ShapeCoeff = field.ShapeCoeff,
                CropType = field.CropType
            };
            return fieldDto;
        }
        public static IEnumerable<FieldDto> MapFieldsToFieldDtos(IEnumerable<Field> fields)
        {
            return fields.Select(MapFieldtoFieldDto);
        }
    }
}
