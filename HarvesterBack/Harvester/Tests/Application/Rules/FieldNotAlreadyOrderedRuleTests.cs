using Harvester.Application.Dtos;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Harvester.Application.Services.OrderRules;
using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Tests.Application.Rules
{
    public class FieldNotAlreadyOrderedRuleTests
    {

        [Fact]
        public async Task CheckRule_ReturnsTrue_WhenAllOrdersThisYearIsNull()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                AllOrdersThisYear = null,
            };

            var rule = new FieldIsNotAlreadyOrderedRule();

            var result = await rule.CheckRule(dto);

           
            Assert.True(result.Success);
            Assert.Null(result.RuleName);
        }

        [Fact]
        public async Task CheckRule_ReturnsTrue_WhenOrderIsEdited()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                AllOrdersThisYear = new List<Order>() { new Order { Id = 2, FieldId = 10, Status = OrderStatus.ACCEPTED } },
                OrderId = 2
            };

            var rule = new FieldIsNotAlreadyOrderedRule();

            var result = await rule.CheckRule(dto);
         
            Assert.True(result.Success);
            Assert.Null(result.RuleName);
        }

        [Fact]
        public async Task CheckRule_ReturnsFalse_WhenFieldIsAlreadyOrderedInDifferentOrder()
        {
            var dto = new OrderInformationForCheckAvailDto
            {
                AllOrdersThisYear = new List<Order>() { new Order { Id = 2, FieldId = 10, Status = OrderStatus.ACCEPTED } },
                Field = new Field() { Id=10}
            };

            var rule = new FieldIsNotAlreadyOrderedRule();

            var result = await rule.CheckRule(dto);

            Assert.False(result.Success);
            Assert.Equal("Field is already ordered", result.RuleName);
        }

        [Fact]
        public async Task CheckRule_ReturnsTrue_WhenAllDataIsCorrect()
        {
            var dto = new OrderInformationForCheckAvailDto
            {
                AllOrdersThisYear = new List<Order>() { 
                    new Order { Id = 2, FieldId = 20, Status = OrderStatus.ACCEPTED },
                    new Order { Id = 3, FieldId = 12, Status = OrderStatus.ACCEPTED },
                    new Order { Id = 5, FieldId = 15, Status = OrderStatus.ACCEPTED }
                },
                Field = new Field() { Id = 10 }
            };

            var rule = new FieldIsNotAlreadyOrderedRule();

            var result = await rule.CheckRule(dto);

            Assert.True(result.Success);
            Assert.Equal("Field is already ordered", result.RuleName);
        }

    }
}
