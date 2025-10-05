using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Tests.Application
{
    public class OrderServiceTests
    {

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenCombineNotFound()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
 
            var rules = new List<IOrderRule> { }; 
  
            var dto = new CreateOrderDto { CombineId = 1, FieldId = 1, OrderDate = DateTime.Today };
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId,true)).ReturnsAsync((Combine?)null);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(new Field());

            var service = new OrderService(orderRepoMock.Object, 
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(dto));

        
            Assert.Equal($"Combine with id: {dto.CombineId} doesn't exist", exception.Message);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Never); 
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenFieldNotFound()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();


            var rules = new List<IOrderRule> {};

            var dto = new CreateOrderDto { CombineId = 1, FieldId = 1, OrderDate = DateTime.Today };
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(new Combine());
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync((Field?)null);

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(dto));


            Assert.Equal($"Field with id: {dto.FieldId} doesn't exist", exception.Message);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Once);
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }
        [Fact]
        public async Task CreateAsync_CreatesOrder_WhenAvailabilityIsTrue()
        {

            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();


            var ruleMock1 = new Mock<IOrderRule>();
            ruleMock1.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var ruleMock2 = new Mock<IOrderRule>();
            ruleMock2.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var rules = new List<IOrderRule> { ruleMock1.Object, ruleMock2.Object };

            var dto = new CreateOrderDto
            {
                CombineId = 1,
                FieldId = 2,
                OrderDate = DateTime.Today,
                StrawProcessingMethod = StrawProcessingMethod.CHOP
            };

            var combine = new Combine {Id=1, BaseHaPerHour = 5, PricePerHectare = 100 };
            var field = new Field { Id= 2, AreaHectares = 10, ShapeCoeff = 1, TerrainCoeff = 1 };

            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(combine);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(field);
            orderRepoMock.Setup(r => r.GetAllFromGivenYearAsync(dto.OrderDate.Year)).ReturnsAsync(new List<Order>());

            var service = new OrderService(orderRepoMock.Object,
               combineRepoMock.Object, fieldRepoMock.Object, rules);

       
            var result = await service.CreateAsync(dto);

      
            Assert.True(result.Success);
            orderRepoMock.Verify(r => r.CreateAsync(It.Is<Order>(o =>
                o.CombineId == dto.CombineId &&
                o.FieldId == dto.FieldId &&
                o.StrawProcessingMethod == dto.StrawProcessingMethod &&
                o.EstimatedPrice > 0
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFalse_WhenOneOfTheRulesIsFalse()
        {

            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();


            var ruleMock1 = new Mock<IOrderRule>();
            ruleMock1.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = false });

            var ruleMock2 = new Mock<IOrderRule>();
            ruleMock2.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var rules = new List<IOrderRule> { ruleMock1.Object, ruleMock2.Object };

            var dto = new CreateOrderDto
            {
                CombineId = 1,
                FieldId = 2,
                OrderDate = DateTime.Today,
                StrawProcessingMethod = StrawProcessingMethod.CHOP
            };

            var combine = new Combine { Id = 1, BaseHaPerHour = 5, PricePerHectare = 100 };
            var field = new Field { Id = 2, AreaHectares = 10, ShapeCoeff = 1, TerrainCoeff = 1 };

            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(combine);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(field);
            orderRepoMock.Setup(r => r.GetAllFromGivenYearAsync(dto.OrderDate.Year)).ReturnsAsync(new List<Order>());

            var service = new OrderService(orderRepoMock.Object,
               combineRepoMock.Object, fieldRepoMock.Object, rules);


            var result = await service.CreateAsync(dto);


            Assert.False(result.Success);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Once);
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenOrderNotFound()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var orderId = 1;

            var rules = new List<IOrderRule> { };

            var dto = new CreateOrderDto { CombineId = 1, FieldId = 1, OrderDate = DateTime.Today };
            orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(new Combine());
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(new Field());

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(orderId, dto));


            Assert.Equal($"Order with id: {orderId} doesn't exist", exception.Message);
            orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Never);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Never);
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenCombineNotFound()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var orderId = 1;

            var rules = new List<IOrderRule> { };

            var dto = new CreateOrderDto { CombineId = 1, FieldId = 1, OrderDate = DateTime.Today };
            orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(new Order());
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync((Combine?)null);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(new Field());

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(orderId, dto));


            Assert.Equal($"Combine with id: {dto.CombineId} doesn't exist", exception.Message);
            orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Never);
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenFieldNotFound()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var orderId = 1;

            var rules = new List<IOrderRule> { };

            var dto = new CreateOrderDto { CombineId = 1, FieldId = 1, OrderDate = DateTime.Today };
            orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(new Order());
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(new Combine());
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync((Field?)null);

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(orderId, dto));


            Assert.Equal($"Field with id: {dto.FieldId} doesn't exist", exception.Message);
            orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Once);
            orderRepoMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }


        [Fact]
        public async Task UpdateAsync_UpdatesOrder_WhenAvailabilityIsTrue()
        {

            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var orderId = 1;

            var ruleMock1 = new Mock<IOrderRule>();
            ruleMock1.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var ruleMock2 = new Mock<IOrderRule>();
            ruleMock2.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var rules = new List<IOrderRule> { ruleMock1.Object, ruleMock2.Object };

            var dto = new CreateOrderDto
            {
                CombineId = 1,
                FieldId = 2,
                OrderDate = DateTime.Today,
                StrawProcessingMethod = StrawProcessingMethod.CHOP
            };

            var combine = new Combine { Id = 1, BaseHaPerHour = 5, PricePerHectare = 100 };
            var field = new Field { Id = 2, AreaHectares = 10, ShapeCoeff = 1, TerrainCoeff = 1 };

            orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(new Order 
            {
                Id = 1,
                CombineId = 1,
                FieldId = 1,
                StrawProcessingMethod = StrawProcessingMethod.LEAVE
            });
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(combine);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(field);
            orderRepoMock.Setup(r => r.GetAllFromGivenYearAsync(dto.OrderDate.Year)).ReturnsAsync(new List<Order>());

            var service = new OrderService(orderRepoMock.Object,
               combineRepoMock.Object, fieldRepoMock.Object, rules);


            var result = await service.UpdateAsync(orderId, dto);


            Assert.True(result.Success);
            orderRepoMock.Verify(r => r.UpdateAsync(It.Is<Order>(o =>
                o.CombineId == dto.CombineId &&
                o.FieldId == dto.FieldId &&
                o.StrawProcessingMethod == dto.StrawProcessingMethod &&
                o.EstimatedPrice > 0
            )), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenOneOfTheRulesIsFalse()
        {

            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var orderId = 1;

            var ruleMock1 = new Mock<IOrderRule>();
            ruleMock1.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = true });

            var ruleMock2 = new Mock<IOrderRule>();
            ruleMock2.Setup(r => r.CheckRule(It.IsAny<OrderInformationForCheckAvailDto>()))
                .ReturnsAsync(new CheckRuleForOrderResponseDto { Success = false });

            var rules = new List<IOrderRule> { ruleMock1.Object, ruleMock2.Object };

            var dto = new CreateOrderDto
            {
                CombineId = 1,
                FieldId = 2,
                OrderDate = DateTime.Today,
                StrawProcessingMethod = StrawProcessingMethod.CHOP
            };

            var combine = new Combine { Id = 1, BaseHaPerHour = 5, PricePerHectare = 100 };
            var field = new Field { Id = 2, AreaHectares = 10, ShapeCoeff = 1, TerrainCoeff = 1 };

            orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(new Order
            {
                Id = 1,
                CombineId = 1,
                FieldId = 1,
                StrawProcessingMethod = StrawProcessingMethod.LEAVE
            });
            combineRepoMock.Setup(r => r.GetByIdAsync(dto.CombineId, true)).ReturnsAsync(combine);
            fieldRepoMock.Setup(r => r.GetByIdAsync(dto.FieldId, true)).ReturnsAsync(field);
            orderRepoMock.Setup(r => r.GetAllFromGivenYearAsync(dto.OrderDate.Year)).ReturnsAsync(new List<Order>());

            var service = new OrderService(orderRepoMock.Object,
               combineRepoMock.Object, fieldRepoMock.Object, rules);


            var result = await service.UpdateAsync(orderId, dto);


            Assert.False(result.Success);
            orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
            combineRepoMock.Verify(r => r.GetByIdAsync(dto.CombineId, true), Times.Once);
            fieldRepoMock.Verify(r => r.GetByIdAsync(dto.FieldId, true), Times.Once);
            orderRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenOrderDoesntExist()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var rules = new List<IOrderRule> { };

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            orderRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Order?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(1));

 
            orderRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesOrder_WhenDataIsValid()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var rules = new List<IOrderRule> { };

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var order = new Order { Id = 1 };
            orderRepoMock
                .Setup(r => r.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            await service.DeleteAsync(order.Id);

            orderRepoMock.Verify(r => r.DeleteAsync(order), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenOrderDoesntExist()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var rules = new List<IOrderRule> { };

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            orderRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(1));
        }

       /* [Fact]
        public async Task GetByIdAsync_ReturnsOrder_WhenOrderExists()
        {
            var orderRepoMock = new Mock<IOrderRepository>();
            var combineRepoMock = new Mock<ICombineRepository>();
            var fieldRepoMock = new Mock<IFieldRepository>();
            var rules = new List<IOrderRule> { };

            var service = new OrderService(orderRepoMock.Object,
                combineRepoMock.Object, fieldRepoMock.Object, rules);

            var order = new Order { Id = 1 };
            orderRepoMock
                .Setup(r => r.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            var orderReponse = await service.GetByIdAsync(order.Id);

            Assert.Equal(order.Id, orderReponse.Id);
        }*/

    }
}
