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
    public class CreateFieldDtoValidatorTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenIdentifierNameIsEmpty(string identifierName)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto { IdentifierName = identifierName };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.IdentifierName);
        }

        [Theory]
        [InlineData("123456_1.2024.1")] // poprawny format
        [InlineData("123456_1.2024.1/2")] // poprawny format z podziałem
        public void CreateFieldDtoValidator_DoesNotReturnValidationErrors_WhenIdentifierNameHasValidFormat(string identifierName)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto { IdentifierName = identifierName };

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(x => x.IdentifierName);
        }

        [Theory]
        [InlineData("ABCDEF_1.2024.1")]
        [InlineData("123456_1-2024-1")]
        [InlineData("1234ii56_1___")]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenIdentifierNameHasInvalidFormat(string identifierName)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto { IdentifierName = identifierName };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.IdentifierName);
        }

        [Fact]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenIdentifierNameIsTooLong()
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto { IdentifierName = new string('A', 101) };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.IdentifierName);
        }

        [Fact]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenCommonNameIsTooLong()
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                CommonName = new string('A', 101)
            };
            var result = validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.CommonName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenAreaHectaresIsInvalid(decimal area)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                AreaHectares = area
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.AreaHectares);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1.5)]
        [InlineData(0)]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenTerrainCoeffIsIncorrect(object coeff)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                TerrainCoeff = coeff != null ? Convert.ToDecimal(coeff) : (decimal?)null
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.TerrainCoeff);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1.5)]
        [InlineData(0)]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenShapeCoeffIsIncorrect(object coeff)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                ShapeCoeff = coeff != null ? Convert.ToDecimal(coeff) : (decimal?)null
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.ShapeCoeff);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenCropTypeIsEmpty(string cropType)
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                CropType = cropType
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.CropType);
        }

        [Fact]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenCenterPointIsNull()
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                CenterPoint = null
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.CenterPoint);
        }

        [Fact]
        public void CreateFieldDtoValidator_ReturnsValidationErrors_WhenBoundaryIsNull()
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                Boundary = null
            };

            var result = validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Boundary);
        }

        [Fact]
        public void CreateFieldDtoValidator_DoesNotReturnValidationErrors_WhenDtoIsValid()
        {
            var validator = new CreateFieldDtoValidator();
            var dto = new CreateFieldDto
            {
                IdentifierName = "123456_1.2024.1",
                CommonName = "TestField",
                AreaHectares = 5.5m,
                TerrainCoeff = 1m,
                ShapeCoeff = 0.9m,
                CropType = "Wheat",
                CenterPoint = new GeoPointDto
                {
                    Type = "Point",
                    Coordinates = [20.0, 50.0] 
                },
                Boundary = new GeoMultiPolygonDto
                {
                    Type = "MultiPolygon",
                    Coordinates =
                    [
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
                }
            };

            var result = validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
