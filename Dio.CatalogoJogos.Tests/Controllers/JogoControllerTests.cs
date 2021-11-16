using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Controllers.v1;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Dio.CatalogoJogos.Tests.Controllers
{
    public class JogoControllerTests
    {
        private readonly Guid _testGuid;
        private readonly JogoInputModel _testInputModel;
        private readonly JogoViewModel _testViewModel;
        private readonly List<JogoViewModel> _emptyList;
        private readonly List<JogoViewModel> _list;

        public JogoControllerTests()
        {
            _testGuid = Guid.NewGuid();
            _testInputModel = new JogoInputModel();
            _testViewModel = new JogoViewModel();
            _emptyList = new List<JogoViewModel>();
            _list = new List<JogoViewModel>() { _testViewModel };
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            serviceMock.Verify(m => m.Obter(1, 5), Times.Once());
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnNoContent_IfServiceReturnsEmptyList()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_emptyList);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnOk_IfServiceReturnedModels()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .ReturnsAsync(_testViewModel);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Inserir_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_testInputModel);

            // Assert
            serviceMock.Verify(m => m.Inserir(_testInputModel), Times.Once());
        }

        [Fact]
        public async Task Inserir_ShouldReturnCreated_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Inserir(It.IsAny<JogoInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_testInputModel);

            // Assert
            result.Result.ShouldBeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Atualizar_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, _testInputModel);

            // Assert
            serviceMock.Verify(m => m.Atualizar(_testGuid, _testInputModel), Times.Once());
        }

        [Fact]
        public async Task Atualizar_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Atualizar(It.IsAny<Guid>(), It.IsAny<JogoInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, _testInputModel);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AtualizarValor_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var valor = 50f;
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, valor);

            // Assert
            serviceMock.Verify(m => m.AtualizarValor(_testGuid, valor), Times.Once());
        }

        [Fact]
        public async Task AtualizarValor_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            float valor = 50f;
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.AtualizarValor(It.IsAny<Guid>(), It.IsAny<float>()))
                .ReturnsAsync(_testViewModel);
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, valor);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Remover_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Remover(_testGuid), Times.Once());
        }

        [Fact]
        public async Task Remover_ShouldReturnOk_IfServiceSucceeded()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Remover(It.IsAny<Guid>()));
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }
    }
}
