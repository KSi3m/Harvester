using FluentValidation.TestHelper;
using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators.Validators
{
    public class CreateOrderDtoValidatorTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CreateOrderDtoValidator_ReturnsValidationErrors_WhenFieldIdIsInvalid(int fieldId)
        {
            var validator = new CreateOrderDtoValidator();

            var model = new CreateOrderDto { FieldId = fieldId, CombineId = 1, OrderDate = DateTime.Today };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FieldId)
                  .WithErrorMessage("FieldId must greater than 0");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CreateOrderDtoValidator_ReturnsValidationErrors_WhenCombineIdIsInvalid(int combineId)
        {
            var validator = new CreateOrderDtoValidator();
            var model = new CreateOrderDto { FieldId = 1, CombineId = combineId, OrderDate = DateTime.Today };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.CombineId)
                  .WithErrorMessage("CombineId must greater than 0");
        }

        [Fact]
        public void CreateOrderDtoValidator_ReturnsValidationErrors_WhenOrderDateIsInPast()
        {
            var validator = new CreateOrderDtoValidator();
            var model = new CreateOrderDto { FieldId = 1, CombineId = 1, OrderDate = DateTime.Today.AddDays(-1) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.OrderDate)
                  .WithErrorMessage("OrderDate cannot be in the past");
        }

        [Fact]
        public void CreateOrderDtoValidator_ReturnsNoValidationErrors_WhenDataIsCorrect()
        {
            var validator = new CreateOrderDtoValidator();
            var model = new CreateOrderDto { FieldId = 1, CombineId = 1, OrderDate = DateTime.Today };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
