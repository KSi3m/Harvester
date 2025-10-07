using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Mappings;
using Harvester.Application.Services;
using Harvester.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Tests.Application
{
    public class CombineServiceTests
    {
        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenCombineDoesntExist()
        {
            var combineRepoMock = new Mock<ICombineRepository>();

            var service = new CombineService(combineRepoMock.Object);

            combineRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(),true))
                .ReturnsAsync((Combine?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(1));


            combineRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Combine>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCombine_WhenDataIsValid()
        {
            var combineRepoMock = new Mock<ICombineRepository>();

            var service = new CombineService(combineRepoMock.Object);

            var combine = new Combine { Id = 1 };
            combineRepoMock
                .Setup(r => r.GetByIdAsync(combine.Id, true))
                .ReturnsAsync(combine);

            await service.DeleteAsync(combine.Id);

            combineRepoMock.Verify(r => r.DeleteAsync(combine), Times.Once);
        }
        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenCombineDoesntExist()
        {
            var combineRepoMock = new Mock<ICombineRepository>();

            var service = new CombineService(combineRepoMock.Object);

            combineRepoMock
                .Setup(r => r.GetByIdAsync(1, false))
                .ReturnsAsync((Combine?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCombine_WhenCombineExists()
        {
            var combineRepoMock = new Mock<ICombineRepository>();

            var service = new CombineService(combineRepoMock.Object);

            var combine = new Combine { Id = 1 };
            combineRepoMock
                .Setup(r => r.GetByIdAsync(combine.Id, false))
                .ReturnsAsync(combine);

            var combineResponse = await service.GetByIdAsync(combine.Id);

            Assert.Equal(combine.Id, combineResponse.Id);
        }

        [Fact]
        public async Task CreateAsync_CreatesCombine_WhenDataIsValid()
        {
            var mockRepository = new Mock<ICombineRepository>();

            var service = new CombineService(mockRepository.Object);

            var dto = new CreateCombineDto
            {
                HeaderLength = 4.5m,
                PricePerHectare = 400
            };
            var expectedCombine = CombineMappings.MapCreateCombineDtoToCombine(dto);

            await service.CreateAsync(dto);

            mockRepository.Verify(
                repo => repo.CreateAsync(It.Is<Combine>(c =>
                    c.HeaderLength == expectedCombine.HeaderLength &&
                    c.PricePerHectare == expectedCombine.PricePerHectare
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenCombineDoesntExist()
        {
            var mockRepository = new Mock<ICombineRepository>();

            var service = new CombineService(mockRepository.Object);
            var combineId = 2;
            var dto = new CreateCombineDto
            {
                HeaderLength = 4.5m,
                PricePerHectare = 400
            };

            var combine = new Combine
            {
                Id = 1,
                HeaderLength = 4.0m,
                PricePerHectare = 600
            };

            mockRepository
                .Setup(r => r.GetByIdAsync(combineId, false))
                .ReturnsAsync((Combine?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(combineId, dto));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCombine_WhenDataIsValid()
        {
            var mockRepository = new Mock<ICombineRepository>();

            var service = new CombineService(mockRepository.Object);
            var combineId = 1;
            var dto = new CreateCombineDto
            {
                HeaderLength = 4.5m,
                PricePerHectare = 400
            };

            var combine = new Combine { 
                Id = 1,
                HeaderLength = 4.0m,
                PricePerHectare = 600
            };

            mockRepository
                .Setup(r => r.GetByIdAsync(combineId, false))
                .ReturnsAsync(combine);

            await service.UpdateAsync(combineId,dto);

            mockRepository.Verify(
                repo => repo.UpdateAsync(It.Is<Combine>(c =>
                    c.HeaderLength == 4.5m &&
                    c.PricePerHectare == 400
                )),
                Times.Once
            );
        }
    }
}
