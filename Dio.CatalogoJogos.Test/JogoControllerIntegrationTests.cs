using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Business.Services;
using Dio.CatalogoJogos.Api.Controllers.v1;
using Dio.CatalogoJogos.Api.Infrastructure.Model.InputModel;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Dio.CatalogoJogos.Test
{
    public class JogoControllerIntegrationTests : IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;

        public JogoControllerIntegrationTests(TestingWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnNoContent_IfServiceReturnsEmptyList()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<JogoViewModel>()
                { });
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnOk_AndReturnCorrectNumberOfModels_IfServiceReturnedModels()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<JogoViewModel>()
                { new JogoViewModel(), new JogoViewModel() });
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            ((result.Result as OkObjectResult).Value as IEnumerable<JogoViewModel>).Count().ShouldBe(2);
        }

        [Fact]
        public async Task ObterPorId_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(guid);

            // Assert
            serviceMock.Verify(m => m.Obter(guid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnOk_AndReturnCorrectModel_IfServiceReturnedModel()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .ReturnsAsync(new JogoViewModel() { Id = guid });
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(guid);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            ((result.Result as OkObjectResult).Value as JogoViewModel).Id.ShouldBe(guid);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnNotFound_IfServiceThrewEntidadeNaoCadastradaException()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .Throws<EntidadeNaoCadastradaException>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Obter(Guid.NewGuid());

            // Assert
            result.Result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Inserir_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var inputModel = new JogoInputModel();
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(inputModel);

            // Assert
            serviceMock.Verify(m => m.Inserir(inputModel), Times.Once());
        }

        [Fact]
        public async Task Inserir_ShouldReturnCreated_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Inserir(It.IsAny<JogoInputModel>()))
                .ReturnsAsync(new JogoViewModel());
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(new JogoInputModel());

            // Assert
            result.Result.ShouldBeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Inserir_ShouldReturnUnprocessableEntity_IfServiceThrewEntidadeJaCadastradaException()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Inserir(It.IsAny<JogoInputModel>()))
                .Throws<EntidadeJaCadastradaException>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(new JogoInputModel());

            // Assert
            result.Result.ShouldBeOfType<UnprocessableEntityObjectResult>();
        }

        [Fact]
        public async Task Atualizar_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var inputModel = new JogoInputModel();
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(guid, inputModel);

            // Assert
            serviceMock.Verify(m => m.Atualizar(guid, inputModel), Times.Once());
        }

        [Fact]
        public async Task Atualizar_ShouldReturnOk_AndReturnCorrectModel_IfServiceReturnedModel()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Atualizar(It.IsAny<Guid>(), It.IsAny<JogoInputModel>()))
                .ReturnsAsync(new JogoViewModel() { Id = guid });
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(guid, new JogoInputModel());

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            ((result.Result as OkObjectResult).Value as JogoViewModel).Id.ShouldBe(guid);
        }

        [Fact]
        public async Task Atualizar_ShouldReturnNotFound_IfServiceThrewEntidadeNaoCadastradaException()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Atualizar(It.IsAny<Guid>(), It.IsAny<JogoInputModel>()))
                .Throws<EntidadeNaoCadastradaException>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(Guid.NewGuid(), new JogoInputModel());

            // Assert
            result.Result.ShouldBeOfType<NotFoundObjectResult>();
        }
        [Fact]
        public async Task AtualizarValor_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(guid, 50);

            // Assert
            serviceMock.Verify(m => m.AtualizarValor(guid, 50), Times.Once());
        }

        [Fact]
        public async Task AtualizarValor_ShouldReturnOk_AndReturnCorrectModel_IfServiceReturnedModel()
        {
            // Arrange
            var guid = Guid.NewGuid();
            float valor = 50;
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.AtualizarValor(It.IsAny<Guid>(), It.IsAny<float>()))
                .ReturnsAsync(new JogoViewModel() { Id = guid, Valor = valor });
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(guid, valor);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            ((result.Result as OkObjectResult).Value as JogoViewModel).Id.ShouldBe(guid);
            ((result.Result as OkObjectResult).Value as JogoViewModel).Valor.ShouldBe(valor);
        }

        [Fact]
        public async Task AtualizarValor_ShouldReturnNotFound_IfServiceThrewEntidadeNaoCadastradaException()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.AtualizarValor(It.IsAny<Guid>(), It.IsAny<float>()))
                .Throws<EntidadeNaoCadastradaException>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Atualizar(Guid.NewGuid(), 50f);

            // Assert
            result.Result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Remover_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<IJogoService>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Remover(guid);

            // Assert
            serviceMock.Verify(m => m.Remover(guid), Times.Once());
        }

        [Fact]
        public async Task Remover_ShouldReturnOk_IfServiceSucceeded()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Remover(It.IsAny<Guid>()));
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Remover(Guid.NewGuid());

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Remover_ShouldReturnNotFound_IfServiceThrewEntidadeNaoCadastradaException()
        {
            // Arrange
            var serviceMock = new Mock<IJogoService>();
            serviceMock.Setup(m => m.Remover(It.IsAny<Guid>()))
                .Throws<EntidadeNaoCadastradaException>();
            var controller = new JogoController(serviceMock.Object);

            // Act
            var result = await controller.Remover(Guid.NewGuid());

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
        }
    }
}
