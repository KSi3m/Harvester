using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Mappings;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Services
{
    public class CombineService(ICombineRepository combineRepository, IEnumerable<IOrderRule> checkRules) : ICombineService
    {
        public async Task<CheckRuleForOrderResponseDto> CheckAvailability(OrderInformationForCheckAvailDto dto)
        {
            foreach(var rule in checkRules)
            {
                var res = await rule.CheckRule(dto);
                if (!res.Success) return res;
            }
            return new CheckRuleForOrderResponseDto { Success = true };
        }

        public async Task CreateAsync(CreateCombineDto dto)
        {
            var combine = new Combine
            {
                Model = dto.Model,
                BaseHaPerHour = dto.BaseHaPerHour,
                HeaderLength = dto.HeaderLength,
                IsAvailable = dto.IsAvailable,
                PricePerHectare = dto.PricePerHectare,
                HasStrawChopper = dto.HasStrawChopper,
                AvailableWorkHours = dto.AvailableWorkHours,
                BaseEfficency = dto.BaseEfficency,
            };
            await combineRepository.CreateAsync(combine);
        }

        public async Task DeleteAsync(int id)
        {
            var combine = await combineRepository.GetByIdAsync(id);
            if(combine == null)
            {
                throw new NotFoundException("Combine doesn't exist");
            }
            await combineRepository.DeleteAsync(combine);
        }

        public async Task<IEnumerable<CombineDto>> GetAllAsync()
        {
            var combines = await combineRepository.GetAllAsync();
            return CombineMappings.MapCombinesToCombineDtos(combines);
        }

        public async Task<CombineDto> GetByIdAsync(int id)
        {
            var combine = await combineRepository.GetByIdAsync(id);
            if (combine == null)
            {
                throw new NotFoundException("Combine doesn't exist");
            }
            return CombineMappings.MapCombineToCombineDto(combine);
        }

        public async Task UpdateAsync(int id, CreateCombineDto dto)
        {
            var combine = await combineRepository.GetByIdAsync(id);

            if (combine == null)
            {
                throw new NotFoundException("Combine doesn't exist");
            }

            combine.Model = dto.Model;
            combine.BaseHaPerHour = dto.BaseHaPerHour;
            combine.HeaderLength = dto.HeaderLength;
            combine.IsAvailable = dto.IsAvailable;
            combine.AvailableWorkHours = dto.AvailableWorkHours;
            combine.BaseEfficency = dto.BaseEfficency;
            combine.PricePerHectare = dto.PricePerHectare;
            combine.HasStrawChopper = dto.HasStrawChopper;

            await combineRepository.UpdateAsync(combine);
        }
    }
}
