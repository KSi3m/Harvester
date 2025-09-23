using Harvester.Application.Dtos;
using Harvester.Application.Services.OrderRules;
using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Tests.Application.Rules
{
    public class CombineScheduleIsNotAlreadyFilledRuleTests
    {
        [Fact]
        public async Task CheckRule_ReturnsTrue_ThereAreNoOrdersForCombine()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                Combine = new Combine() { Id = 1, Orders = new List<Order>()},
            };

            var rule = new CombineScheduleIsNotAlreadyFilledRule();

            var result = await rule.CheckRule(dto);


            Assert.True(result.Success);
            Assert.Null(result.RuleName);
        }

        [Fact]
        public async Task CheckRule_ReturnsFalse_WhenOrderIsExceedingCombineTimeLimit()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                Combine = new Combine()
                {
                    Id = 1,
                    AvailableWorkHours = 5,
                    Orders = new List<Order>() {
                        new Order() {Id = 1, Status = OrderStatus.ACCEPTED, ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 100},
                        new Order() {Id = 2, Status = OrderStatus.ACCEPTED, ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 20},
                        new Order() {Id = 3, Status = OrderStatus.ACCEPTED, ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 50}
                    }
                },
                EstimatedTime = 150,
                OrderDate = new DateOnly(2025, 08, 12)
            };

            var rule = new CombineScheduleIsNotAlreadyFilledRule();

            var result = await rule.CheckRule(dto);


            Assert.False(result.Success);
            Assert.Equal("Combine Schedule Is Already Filled", result.RuleName);
            Assert.Equal("You can't schedule any more orders for this day. Please try the next day", result.Details);
        }

        [Fact]
        public async Task CheckRule_ReturnsTrue_WhenOrderIsWithinCombineTimeLimit()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                Combine = new Combine()
                {
                    Id = 1,
                    AvailableWorkHours = 5,
                    Orders = new List<Order>() {
                        new Order() {
                            Id = 1, Status = OrderStatus.ACCEPTED, 
                            ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 20},
                        new Order() {
                            Id = 2, Status = OrderStatus.ACCEPTED, 
                            ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 20},
                        new Order() {
                            Id = 3, Status = OrderStatus.ACCEPTED, 
                            ScheduledDate = new DateTime(2025,08,12), EstimatedTime = 50}
                    }
                },
                EstimatedTime = 150,
                OrderDate = new DateOnly(2025, 08, 12)
            };

            var rule = new CombineScheduleIsNotAlreadyFilledRule();

            var result = await rule.CheckRule(dto);


            Assert.True(result.Success);
        }

        [Fact]
        public async Task CheckRule_ReturnsFalse_WhenOrderIsExceedingCombineTimeLimitAfterEditing()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                Combine = new Combine()
                {
                    Id = 1,
                    AvailableWorkHours = 5,
                    Orders = new List<Order>() {
                        new Order() {
                            Id = 1, Status = OrderStatus.ACCEPTED, 
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 100 },
                        new Order() { 
                            Id = 2, Status = OrderStatus.ACCEPTED,
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 20 },
                        new Order() { Id = 3, Status = OrderStatus.ACCEPTED,
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 50 }
                    }
                },
                EstimatedTime = 210,
                OrderDate = new DateOnly(2025, 08, 12),
                OrderId = 1
            };

            var rule = new CombineScheduleIsNotAlreadyFilledRule();

            var result = await rule.CheckRule(dto);


            Assert.False(result.Success);
            Assert.Equal("Combine Schedule Is Already Filled", result.RuleName);
            Assert.Equal("You can't schedule any more orders for this day. Please try the next day", result.Details);
        }

        [Fact]
        public async Task CheckRule_ReturnsTrue_WhenOrderIsWithinCombineTimeLimitAfterEditing()
        {

            var dto = new OrderInformationForCheckAvailDto
            {
                Combine = new Combine()
                {
                    Id = 1,
                    AvailableWorkHours = 5,
                    Orders = new List<Order>() {
                        new Order() {
                            Id = 1, Status = OrderStatus.ACCEPTED,
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 100 },
                        new Order() {
                            Id = 2, Status = OrderStatus.ACCEPTED,
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 20 },
                        new Order() { Id = 3, Status = OrderStatus.ACCEPTED,
                            ScheduledDate = new DateTime(2025, 08, 12), EstimatedTime = 50 }
                    }
                },
                EstimatedTime = 190,
                OrderDate = new DateOnly(2025, 08, 12),
                OrderId = 1
            };

            var rule = new CombineScheduleIsNotAlreadyFilledRule();

            var result = await rule.CheckRule(dto);

            Assert.True(result.Success);
        }
    }
}
