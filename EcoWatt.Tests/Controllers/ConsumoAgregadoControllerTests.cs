using EcoWatt.API.Controllers;
using EcoWatt.API.DTOs;
using EcoWatt.API.Models;
using EcoWatt.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                DataConsumo = DateTime.Now,
                Descricao = "Novo Consumo"
            };

            var consumo = new ConsumoAgregado
            {
                // Id será atribuído pelo mock
                RelatorioId = consumoCreateDto.RelatorioId,
                DispositivoId = consumoCreateDto.DispositivoId,
                Consumo = consumoCreateDto.Consumo,
                DataConsumo = consumoCreateDto.DataConsumo,
                Descricao = consumoCreateDto.Descricao
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId))
                        .ReturnsAsync(true);

            _mockService.Setup(service => service.AddConsumoAsync(It.IsAny<ConsumoAgregado>()))
                        .Callback<ConsumoAgregado>(ca => ca.Id = 1) // Atribui o Id
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnConsumo = Assert.IsType<ConsumoAgregado>(createdAtActionResult.Value);
            Assert.Equal(1, returnConsumo.Id); // Verifica se o Id é 1
            Assert.Equal(consumo.Descricao, returnConsumo.Descricao);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var consumoCreateDto = new ConsumoAgregadoCreateDto
            {
                // Propriedades ausentes para invalidar o ModelState
            };
            _controller.ModelState.AddModelError("Nome", "Required");

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
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
                DataConsumo = System.DateTime.Now,
                Descricao = "Novo Consumo"
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoCreateDto.DispositivoId))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(consumoCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Dispositivo com Id {consumoCreateDto.DispositivoId.Value} não encontrado.", badRequestResult.Value);
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
                DataConsumo = System.DateTime.Now,
                Descricao = "Consumo Atualizado"
            };

            var consumo = new ConsumoAgregado
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
            _mockService.Setup(service => service.UpdateConsumoAsync(consumo))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
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
                DataConsumo = System.DateTime.Now,
                Descricao = "Consumo Atualizado"
            };

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
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
                DataConsumo = System.DateTime.Now,
                Descricao = "Consumo Atualizado"
            };

            _mockService.Setup(service => service.DispositivoExistsAsync(consumoUpdateDto.DispositivoId))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(testId, consumoUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Dispositivo com Id {consumoUpdateDto.DispositivoId.Value} não encontrado.", badRequestResult.Value);
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
        }
    }
}