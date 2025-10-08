using FluentValidation.TestHelper;
using Harvester.Application.Dtos;
using Harvester.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Tests.Application.Validators
{
    public class CreateCombineDtoValidatorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenModelIsEmpty(string model)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { Model = model };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Model);
        }

        [Fact]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenModelIsTooLong()
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { Model = new string('x', 101) };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Model);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(-5)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenBaseHaPerHourIsInvalid(decimal baseHaPerHour)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { BaseHaPerHour = baseHaPerHour };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.BaseHaPerHour);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenHeaderLengthIsTooSmall(decimal headerLength)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { HeaderLength = headerLength };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.HeaderLength);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(5000)]
        [InlineData(10000)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenPricePerHectareIsInvalid(int price)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { PricePerHectare = price };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.PricePerHectare);
        }

        [Fact]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenIsAvailableIsNull()
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto {  IsAvailable = null};

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.IsAvailable);
        }

        [Fact]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenHasStrawChopperIsNull()
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { HasStrawChopper = null };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.HasStrawChopper);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenAvailableWorkHoursIsInvalid(int availableHours)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { AvailableWorkHours = availableHours };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.AvailableWorkHours);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(1.5)]
        [InlineData(10)]
        public void CreateCombineDtoValidator_ReturnsValidationErrors_WhenBaseEfficencyIsInvalid(decimal baseEfficency)
        {
            var validator = new CreateCombineDtoValidator();
            var dto = new CreateCombineDto { BaseEfficency = baseEfficency };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.BaseEfficency);
        }
    }
}
