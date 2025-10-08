using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
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
    public class FieldServiceTests
    {
        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenFieldDoesntExist()
        {
            var fieldRepoMock = new Mock<IFieldRepository>();

            var service = new FieldService(fieldRepoMock.Object);

            fieldRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), true))
                .ReturnsAsync((Field?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(1));

            fieldRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Field>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesField_WhenDataIsValid()
        {
            var fieldRepoMock = new Mock<IFieldRepository>();

            var service = new FieldService(fieldRepoMock.Object);

            var field = new Field { Id = 1 }; 
            fieldRepoMock
                .Setup(r => r.GetByIdAsync(field.Id, true))
                .ReturnsAsync(field);

            await service.DeleteAsync(field.Id);

            fieldRepoMock.Verify(r => r.DeleteAsync(field), Times.Once);  
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WhenFieldDoesntExist()
        {
            var fieldRepoMock = new Mock<IFieldRepository>();

            var service = new FieldService(fieldRepoMock.Object);

            fieldRepoMock
                .Setup(r => r.GetByIdAsync(1, true))
                .ReturnsAsync((Field?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsField_WhenFieldExists()
        {
            var fieldRepoMock = new Mock<IFieldRepository>();

            var service = new FieldService(fieldRepoMock.Object);

            var field = new Field { Id = 1 };
            fieldRepoMock
                .Setup(r => r.GetByIdAsync(field.Id,false))
                .ReturnsAsync(field);

            var fieldResponse = await service.GetByIdAsync(field.Id);

            Assert.Equal(field.Id, fieldResponse.Id);
        }

        [Fact]
        public async Task CreateAsync_CreatesField_WhenDataIsValid()
        {
            var mockRepository = new Mock<IFieldRepository>();

            var service = new FieldService(mockRepository.Object);

            var dto = new CreateFieldDto
            {
                IdentifierName = "123456_1.2024.1",
                CommonName = "Example"
            };
            var expectedField = FieldMappings.MapCreateFieldDtoToField(dto);

            await service.CreateAsync(dto);

            mockRepository.Verify(
                repo => repo.CreateAsync(It.Is<Field>(f =>
                    f.IdentifierName == expectedField.IdentifierName &&
                    f.CommonName == expectedField.CommonName
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenFieldDoesntExist()
        {
            var mockRepository = new Mock<IFieldRepository>();

            var service = new FieldService(mockRepository.Object);
            var fieldId = 2;
            var dto = new CreateFieldDto
            {
                IdentifierName = "123456_1.2024.1/1",
                CommonName = "Example"
            };

            mockRepository
                .Setup(r => r.GetByIdAsync(fieldId, false))
                .ReturnsAsync((Field?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(fieldId,dto));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesField_WhenDataIsValid()
        {
            var mockRepository = new Mock<IFieldRepository>();

            var service = new FieldService(mockRepository.Object);
            var fieldId = 1;
            var dto = new CreateFieldDto
            {
                IdentifierName = "123456_1.2024.1/1",
                CommonName = "Example"
            };

            var field = new Field { Id = 1, IdentifierName = "123456_1.2024.1", CommonName = "test" };
            mockRepository
                .Setup(r => r.GetByIdAsync(fieldId, false))
                .ReturnsAsync(field);

            await service.UpdateAsync(fieldId, dto);

            mockRepository.Verify(
                repo => repo.UpdateAsync(It.Is<Field>(f =>
                    f.IdentifierName == "123456_1.2024.1/1" &&
                    f.CommonName == "Example"
                )),
                Times.Once
            );
        }
    }
}
