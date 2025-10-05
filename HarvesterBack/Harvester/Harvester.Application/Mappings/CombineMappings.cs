using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Mappings
{
    public class CombineMappings
    {
        public static CombineDto MapCombineToCombineDto(Combine combine)
        {
            var combineDto = new CombineDto
            {
                Id = combine.Id,
                Model = combine.Model,
                BaseHaPerHour = combine.BaseHaPerHour,
                HeaderLength = combine.HeaderLength,
                IsAvailable = combine.IsAvailable,
                AvailableWorkHours = combine.AvailableWorkHours,
                PricePerHectare = combine.PricePerHectare,
                HasStrawChopper = combine.HasStrawChopper,
                BaseEfficency = combine.BaseEfficency,
            };
            return combineDto;
        }
        public static IEnumerable<CombineDto> MapCombinesToCombineDtos(IEnumerable<Combine> combines)
        {
            return combines.Select(MapCombineToCombineDto);
        }
        public static Combine MapCreateCombineDtoToCombine(CreateCombineDto dto)
        {
            var combine = new Combine
            {
                Model = dto.Model,
                BaseHaPerHour = dto.BaseHaPerHour,
                HeaderLength = dto.HeaderLength,
                IsAvailable = dto.IsAvailable ?? false,
                PricePerHectare = dto.PricePerHectare,
                HasStrawChopper = dto.HasStrawChopper ?? false,
                AvailableWorkHours = dto.AvailableWorkHours,
                BaseEfficency = dto.BaseEfficency,
            };
            return combine;
        }
    }
}
