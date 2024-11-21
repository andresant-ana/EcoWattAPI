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
    public class RelatoriosControllerTests
    {
        private readonly Mock<IRelatorioService> _mockService;
        private readonly RelatoriosController _controller;

        public RelatoriosControllerTests()
        {
            _mockService = new Mock<IRelatorioService>();
            _controller = new RelatoriosController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfRelatorios()
        {
            // Arrange
            var relatorios = new List<Relatorio>
            {
                new Relatorio { Id = 1, Nome = "Relatorio 1" },
                new Relatorio { Id = 2, Nome = "Relatorio 2" }
            };
            _mockService.Setup(service => service.GetAllRelatoriosAsync()).ReturnsAsync(relatorios);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRelatorios = Assert.IsAssignableFrom<IEnumerable<Relatorio>>(okResult.Value);
            Assert.Equal(2, ((List<Relatorio>)returnRelatorios).Count);
            _mockService.Verify(service => service.GetAllRelatoriosAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithRelatorio()
        {
            // Arrange
            int testId = 1;
            var relatorio = new Relatorio { Id = testId, Nome = "Relatorio Teste" };
            _mockService.Setup(service => service.GetRelatorioByIdAsync(testId)).ReturnsAsync(relatorio);

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRelatorio = Assert.IsType<Relatorio>(okResult.Value);
            Assert.Equal(testId, returnRelatorio.Id);
            Assert.Equal("Relatorio Teste", returnRelatorio.Nome);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(testId), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenRelatorioDoesNotExist()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetRelatorioByIdAsync(testId)).ReturnsAsync((Relatorio)null);

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(testId), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var relatorioCreateDto = new RelatorioCreateDto { Nome = "Novo Relatorio" };
            var relatorio = new Relatorio { Id = 1, Nome = relatorioCreateDto.Nome };

            _mockService.Setup(service => service.AddRelatorioAsync(It.IsAny<Relatorio>()))
                        .Callback<Relatorio>(r => r.Id = 1)
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(relatorioCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnRelatorio = Assert.IsType<Relatorio>(createdAtActionResult.Value);
            Assert.Equal(1, returnRelatorio.Id);
            Assert.Equal(relatorio.Nome, returnRelatorio.Nome);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            _mockService.Verify(service => service.AddRelatorioAsync(It.IsAny<Relatorio>()), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNoContentResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int testId = 1;
            var relatorioUpdateDto = new RelatorioUpdateDto { Id = testId, Nome = "Relatorio Atualizado" };
            var relatorio = new Relatorio { Id = testId, Nome = relatorioUpdateDto.Nome };

            _mockService.Setup(service => service.GetRelatorioByIdAsync(testId)).ReturnsAsync(relatorio);
            _mockService.Setup(service => service.UpdateRelatorioAsync(relatorio)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(testId, relatorioUpdateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.UpdateRelatorioAsync(relatorio), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            int testId = 1;
            var relatorioUpdateDto = new RelatorioUpdateDto { Id = 2, Nome = "Relatorio Atualizado" };

            // Act
            var result = await _controller.Update(testId, relatorioUpdateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID na rota não corresponde ao ID no corpo da requisição.", badRequestResult.Value);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(It.IsAny<int>()), Times.Never);
            _mockService.Verify(service => service.UpdateRelatorioAsync(It.IsAny<Relatorio>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            int testId = 1;
            var relatorio = new Relatorio { Id = testId, Nome = "Relatorio a Ser Deletado" };
            _mockService.Setup(service => service.GetRelatorioByIdAsync(testId)).ReturnsAsync(relatorio);
            _mockService.Setup(service => service.DeleteRelatorioAsync(testId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.DeleteRelatorioAsync(testId), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRelatorioDoesNotExist()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(service => service.GetRelatorioByIdAsync(testId)).ReturnsAsync((Relatorio)null);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockService.Verify(service => service.GetRelatorioByIdAsync(testId), Times.Once);
            _mockService.Verify(service => service.DeleteRelatorioAsync(testId), Times.Never);
        }
    }
}