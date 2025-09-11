using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Mappings;
using Harvester.Domain.Models;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Harvester.Application.Services
{
    public class FieldService(IFieldRepository fieldRepository) : IFieldService
    {
        public async Task CreateAsync(CreateFieldDto dto)
        {
            var field = FieldMappings.MapCreateFieldDtoToField(dto);
            await fieldRepository.CreateAsync(field);
        }

        public async Task DeleteAsync(int id)
        {
            var field = await fieldRepository.GetByIdAsync(id);
            if (field == null)
            {
                throw new NotFoundException("Field doesn't exist");
            }
            await fieldRepository.DeleteAsync(field);
        }

        public async Task<IEnumerable<FieldDto>> GetAllAsync()
        {
            var fields = await fieldRepository.GetAllAsync();
            return FieldMappings.MapFieldsToFieldDtos(fields);
        }

        public async Task<FieldDto> GetByIdAsync(int id)
        {
            var field = await fieldRepository.GetByIdAsync(id);
            if (field == null)
            {
                throw new NotFoundException("Field doesn't exist");
            }

            return FieldMappings.MapFieldtoFieldDto(field);
        }

        public async Task UpdateAsync(int id, CreateFieldDto dto)
        {
            var field = await fieldRepository.GetByIdAsync(id);

            if (field == null)
            {
                throw new NotFoundException("Field doesn't exist");
            }

            field.IdentifierName = dto.IdentifierName;
            field.CommonName = dto.CommonName;
            field.AreaHectares = dto.AreaHectares;
            field.TerrainCoeff = dto.TerrainCoeff;
            field.ShapeCoeff = dto.ShapeCoeff;
            field.CropType = dto.CropType;
            field.CenterPoint = GeoJSONMappings.MapGeoPointDtoToPoint(dto.CenterPoint);
            field.Boundary = GeoJSONMappings.MapMultiPolygonDtoToMultiPolygon(dto.Boundary);

            await fieldRepository.UpdateAsync(field);
        }
    }
}
