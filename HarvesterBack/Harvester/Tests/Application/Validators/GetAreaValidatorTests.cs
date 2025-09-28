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
    public class GetAreaValidatorTests
    {
        [Theory]
        [InlineData("WWERERWWERWERWERWERWERWERWERWERWERWERWERWERWEr_1.2024.1")]
        [InlineData("123456_1-2024-1")]
        [InlineData("1234ii56_1___")]
        public void GetAreaValidator_ReturnsValidationErrors_WhenIdentityNameIsInvalid(string idName)
        {
            var validator = new GetAreaValidator();

            var result = validator.TestValidate(idName);

            result.ShouldHaveValidationErrorFor(x => x);
        }
        [Theory]
        [InlineData("123456_1.2028.1")] // poprawny format
        [InlineData("123456_1.2023.1/5")]
        public void GetAreaValidator_ReturnsNoValidationErrors_WhenIdentityNameIsValid(string idName)
        {
            var validator = new GetAreaValidator();

            var result = validator.TestValidate(idName);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }
    }
}
