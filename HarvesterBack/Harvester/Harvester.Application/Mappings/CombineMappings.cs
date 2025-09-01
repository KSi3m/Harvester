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
                BaseEfficency = combine.BaseEfficency,
            };
            return combineDto;
        }
        public static IEnumerable<CombineDto> MapCombinesToCombineDtos(IEnumerable<Combine> combines)
        {
            return combines.Select(MapCombineToCombineDto);
        }
    }
}
