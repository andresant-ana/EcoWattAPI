using EcoWatt.API.Controllers;
using EcoWatt.API.DTOs;
using EcoWatt.API.Models;
using EcoWatt.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoWatt.API.Interfaces.Services;

namespace EcoWatt.Tests.Controllers
{
    public class ConsumoAgregadoControllerTests
    {
        private readonly Mock<IConsumoAgregadoService> _mockService;
        private readonly ConsumoAgregadoController _controller;

        public ConsumoAgregadoControllerTests()
        {
            _mockService = new Mock<IConsumoAgregadoService>();
            _controller = new ConsumoAgregadoController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfConsumos()
        {
            // Arrange
            var consumos = new List<ConsumoAgregado>
            {
                new ConsumoAgregado { Id = 1, Descricao = "Consumo 1" },
                new ConsumoAgregado { Id = 2, Descricao = "Consumo 2" }
            };
            _mockService.Setup(service => service.GetAllConsumosAsync()).ReturnsAsync(consumos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnConsumos = Assert.IsAssignableFrom<IEnumerable<ConsumoAgregado>>(okResult.Value);
            Assert.Equal(2, ((List<ConsumoAgregado>)returnConsumos).Count);
            _mockService.Verify(service => service.GetAllConsumosAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithConsumo()
        {
            // Arrange
            int testId = 1;
            var consumo = new ConsumoAgregado { Id = testId, Descricao = "Consumo Teste" };
            _mockService.Setup(service => service.GetConsumoByIdAsync(testId)).ReturnsAsync(consumo);

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnConsumo = Assert.IsType<ConsumoAgregado>(okResult.Value);
            Assert.Equal(testId, returnConsumo.Id);
            Assert.Equal("Consumo Teste", returnConsumo.Descricao);
            _mockService.Verify(service => service.GetConsumoByIdAsync(testId), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenConsumoDoesNotExist()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetConsumoByIdAsync(testId)).ReturnsAsync((ConsumoAgregado)null);

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _mockService.Verify(service => service.GetConsumoByIdAsync(testId), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var consumoCreateDto = new ConsumoAgregadoCreateDto
            {
                RelatorioId = 1,
                DispositivoId = 2,
                Consumo = 150.75,
                DataConsumo = System.DateTime.UtcNow,
                Descricao = "Novo Consumo"
            };

            var consumo = new ConsumoAgregado
            {
                RelatorioId = consumoCreateDto.RelatorioId,
                DispositivoId = consumoCreateDto.DispositivoId,
                Consumo = consumoCreateDto.Consumo,
                DataConsumo = consumoCreateDto.DataConsumo,
                Descricao = consumoCreateDto.Descricao
                // Id será atribuído pelo mock
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId))
                        .ReturnsAsync(true);
            _mockService.Setup(service => service.AddConsumoAsync(It.IsAny<ConsumoAgregado>()))
                        .Callback<ConsumoAgregado>(ca => ca.Id = 1)
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnConsumo = Assert.IsType<ConsumoAgregado>(createdAtActionResult.Value);
            Assert.Equal(1, returnConsumo.Id);
            Assert.Equal(consumo.Descricao, returnConsumo.Descricao);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            _mockService.Verify(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId), Times.Once);
            _mockService.Verify(service => service.AddConsumoAsync(It.IsAny<ConsumoAgregado>()), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var consumoCreateDto = new ConsumoAgregadoCreateDto
            {
                // Propriedades ausentes para invalidar o ModelState
            };
            _controller.ModelState.AddModelError("Descricao", "Required");

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _mockService.Verify(service => service.DispositivoExistsAsync(It.IsAny<int?>()), Times.Never);
            _mockService.Verify(service => service.AddConsumoAsync(It.IsAny<ConsumoAgregado>()), Times.Never);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenDispositivoDoesNotExist()
        {
            // Arrange
            var consumoCreateDto = new ConsumoAgregadoCreateDto
            {
                RelatorioId = 1,
                DispositivoId = 2,
                Consumo = 150.75,
                DataConsumo = System.DateTime.UtcNow,
                Descricao = "Novo Consumo"
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal($"Dispositivo com Id {consumoCreateDto.DispositivoId} não encontrado.", badRequestResult.Value);
            _mockService.Verify(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId), Times.Once);
            _mockService.Verify(service => service.AddConsumoAsync(It.IsAny<ConsumoAgregado>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int testId = 1;
            var consumoUpdateDto = new ConsumoAgregadoUpdateDto
            {
                Id = testId,
                RelatorioId = 1,
                DispositivoId = 2,
                Consumo = 200.50,
                DataConsumo = System.DateTime.UtcNow,
                Descricao = "Consumo Atualizado"
            };
            var consumoExistente = new ConsumoAgregado
            {
                Id = testId,
                RelatorioId = consumoUpdateDto.RelatorioId,
                DispositivoId = consumoUpdateDto.DispositivoId,
                Consumo = consumoUpdateDto.Consumo,
                DataConsumo = consumoUpdateDto.DataConsumo,
                Descricao = consumoUpdateDto.Descricao
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoUpdateDto.DispositivoId))
                        .ReturnsAsync(true);
            _mockService.Setup(service => service.GetConsumoByIdAsync(testId))
                        .ReturnsAsync(consumoExistente);
            _mockService.Setup(service => service.UpdateConsumoAsync(consumoExistente))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(service => service.DispositivoExistsAsync(consumoUpdateDto.DispositivoId), Times.Once);
            _mockService.Verify(service => service.GetConsumoByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.UpdateConsumoAsync(consumoExistente), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            int testId = 1;
            var consumoUpdateDto = new ConsumoAgregadoUpdateDto
            {
                Id = 2, // ID diferente
                RelatorioId = 1,
                DispositivoId = 2,
                Consumo = 200.50,
                DataConsumo = System.DateTime.UtcNow,
                Descricao = "Consumo Atualizado"
            };

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID na rota não corresponde ao ID no corpo da requisição.", badRequestResult.Value);
            _mockService.Verify(service => service.DispositivoExistsAsync(It.IsAny<int?>()), Times.Never);
            _mockService.Verify(service => service.GetConsumoByIdAsync(It.IsAny<int>()), Times.Never);
            _mockService.Verify(service => service.UpdateConsumoAsync(It.IsAny<ConsumoAgregado>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenDispositivoDoesNotExist()
        {
            // Arrange
            int testId = 1;
            var consumoUpdateDto = new ConsumoAgregadoUpdateDto
            {
                Id = testId,
                RelatorioId = 1,
                DispositivoId = 2,
                Consumo = 200.50,
                DataConsumo = System.DateTime.UtcNow,
                Descricao = "Consumo Atualizado"
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoUpdateDto.DispositivoId))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Dispositivo com Id {consumoUpdateDto.DispositivoId} não encontrado.", badRequestResult.Value);
            _mockService.Verify(service => service.DispositivoExistsAsync(consumoUpdateDto.DispositivoId), Times.Once);
            _mockService.Verify(service => service.GetConsumoByIdAsync(It.IsAny<int>()), Times.Never);
            _mockService.Verify(service => service.UpdateConsumoAsync(It.IsAny<ConsumoAgregado>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            int testId = 1;
            var consumo = new ConsumoAgregado { Id = testId, Descricao = "Consumo a Ser Deletado" };
            _mockService.Setup(service => service.GetConsumoByIdAsync(testId)).ReturnsAsync(consumo);
            _mockService.Setup(service => service.DeleteConsumoAsync(testId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(service => service.GetConsumoByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.DeleteConsumoAsync(testId), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenConsumoDoesNotExist()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetConsumoByIdAsync(testId)).ReturnsAsync((ConsumoAgregado)null);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockService.Verify(service => service.GetConsumoByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.DeleteConsumoAsync(It.IsAny<int>()), Times.Never);
        }
    }
}