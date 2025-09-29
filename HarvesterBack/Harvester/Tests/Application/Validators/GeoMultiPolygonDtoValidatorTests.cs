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
    public class GeoMultiPolygonDtoValidatorTests
    {
        [Fact]
        public void GeoMultiPolygonDtoValidator_ReturnsValidationErrors_WhenTypeIsInvalid()
        {
            var validator = new GeoMultiPolygonDtoValidator();
            var dto = new GeoMultiPolygonDto { Type = "MMDDSFs", Coordinates = [] };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Fact]
        public void GeoMultiPolygonDtoValidator_ReturnsValidationErrors_WhenCoordinatesAreEmpty()
        {
            var validator = new GeoMultiPolygonDtoValidator();
            var dto = new GeoMultiPolygonDto { Type = "MultiPolygon", Coordinates = [] };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Coordinates);
        }

        [Fact]
        public void GeoMultiPolygonDtoValidator_DoesNotReturnValidationErrors_WhenDataIsValid()
        {
            var validator = new GeoMultiPolygonDtoValidator();
            var dto = new GeoMultiPolygonDto { Type = "MultiPolygon", Coordinates = [
                        [
                            [
                                [20.0, 50.0],
                                [20.1, 50.0],
                                [20.1, 50.1],
                                [20.0, 50.1],
                                [20.0, 50.0]
                            ]
                        ]
                    ]
            };

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
