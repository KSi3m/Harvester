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
    public class GeoPointDtoValidatorTests
    {
        [Fact]
        public void GeoPointDtoValidator_ReturnsValidationErrors_WhenTypeIsInvalid()
        {
            var validator = new GeoPointDtoValidator();
            var dto = new GeoPointDto { Type = "Pointxx", Coordinates = [] };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [InlineData(new double[] { })]
        [InlineData(new double[] {1,2,3,5 })]
        public void GeoPointDtoValidator_ReturnsValidationErrors_WhenCoordinatesAreInvalid(double[] coords)
        {
            var validator = new GeoPointDtoValidator();
            var dto = new GeoPointDto { Type = "Point", Coordinates = coords };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Coordinates);
        }

        [Fact]
        public void GeoPointDtoValidator_DoesNotReturnValidationErrors_WhenDataIsValid()
        {
            var validator = new GeoPointDtoValidator();
            var dto = new GeoPointDto { Type = "Point", Coordinates = [50.2,82.5] };

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
