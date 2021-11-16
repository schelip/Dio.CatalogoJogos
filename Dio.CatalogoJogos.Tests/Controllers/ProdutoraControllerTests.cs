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
    public class ProdutoraControllerTests
    {
        private readonly Guid _testGuid;
        private readonly string _validISO;
        private readonly ProdutoraInputModel _validInputModel;
        private readonly ProdutoraViewModel _testViewModel;
        private readonly List<ProdutoraViewModel> _emptyList;
        private readonly List<ProdutoraViewModel> _list;

        public ProdutoraControllerTests()
        {
            _testGuid = Guid.NewGuid();
            _validISO = "US";
            _validInputModel = new ProdutoraInputModel()
            {
                ISOPais = "US",
            };
            _testViewModel = new ProdutoraViewModel();
            _emptyList = new List<ProdutoraViewModel>();
            _list = new List<ProdutoraViewModel>() { _testViewModel };
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            serviceMock.Verify(m => m.Obter(1, 5), Times.Once());
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnNoContent_IfServiceReturnsEmptyList()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_emptyList);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnOk_IfServiceReturnedModels()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterPorPais_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_list);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_validISO);

            // Assert
            serviceMock.Verify(m => m.Obter(_validISO), Times.Once);
        }

        [Fact]
        public async Task ObterPorPais_ShouldReturnNoContent_IfServiceReturnedEmptyList()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_emptyList);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_validISO);

            // Assert
            result.Result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ObterPorPais_ShouldReturnOk_IfServiceReturnedModels()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_list);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_validISO);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .ReturnsAsync(_testViewModel);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Inserir_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_validInputModel);

            // Assert
            serviceMock.Verify(m => m.Inserir(_validInputModel), Times.Once());
        }

        [Fact]
        public async Task Inserir_ShouldReturnCreated_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Inserir(It.IsAny<ProdutoraInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_validInputModel);

            // Assert
            result.Result.ShouldBeOfType<CreatedResult>();
        }
        
        [Fact]
        public async Task Atualizar_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, _validInputModel);

            // Assert
            serviceMock.Verify(m => m.Atualizar(_testGuid, _validInputModel), Times.Once());
        }

        [Fact]
        public async Task Atualizar_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Atualizar(It.IsAny<Guid>(), It.IsAny<ProdutoraInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(_testGuid, _validInputModel);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Remover_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Remover(_testGuid), Times.Once());
        }

        [Fact]
        public async Task Remover_ShouldReturnOk_IfServiceSucceeded()
        {
            // Arrange
            var serviceMock = new Mock<IProdutoraService>();
            serviceMock.Setup(m => m.Remover(It.IsAny<Guid>()));
            var controller = new ProdutoraController(serviceMock.Object);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }
    }
}
