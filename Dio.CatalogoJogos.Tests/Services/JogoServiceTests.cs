using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Business.Services;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Moq;
using Shouldly;
using Xunit;

namespace Dio.CatalogoJogos.Test.Services
{
    public class JogoServiceTests
    {
        private readonly Guid _testGuid;
        private readonly Produtora _testProdutora;
        private readonly Jogo _testEntity;
        private readonly JogoInputModel _testInputModel;

        public JogoServiceTests()
        {
            _testGuid = Guid.NewGuid();
            var pGuid = Guid.NewGuid();
            _testProdutora = new Produtora()
            {
                Id = pGuid
            };
            _testEntity = new Jogo()
            {
                Id = _testGuid,
                Nome = "JogoTeste",
                Ano = 1234,
                ProdutoraId = pGuid,
                Produtora = _testProdutora,
                UsuarioJogos = new List<UsuarioJogo>() { },
                Valor = 50
            };
            _testInputModel = new JogoInputModel()
            {
                Nome = "JogoTeste",
                Ano = 1234,
                ProdutoraId = pGuid,
                Valor = 50
            };
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Jogo>());
            var service = new JogoService(repositoryMock.Object);

            // Act
            await service.Obter(1, 5);

            // Assert
            repositoryMock.Verify(m => m.Obter(1, 5), Times.Once);
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnViewModelList()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Jogo>());
            var service = new JogoService(repositoryMock.Object);

            // Act
            var result = await service.Obter(1, 5);

            // Assert
            result.ShouldBeOfType<List<JogoViewModel>>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            var service = new JogoService(repositoryMock.Object);

            // Act
            await service.Obter(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            var service = new JogoService(repositoryMock.Object);

            // Act
            var result = await service.Obter(_testGuid);

            // Assert
            result.ShouldBeOfType<JogoViewModel>();
        }

        [Fact]
        public async Task ObterPorId_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Jogo)null);
            var service = new JogoService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Obter(_testGuid));
        }

        [Fact]
        public async Task Inserir_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(_testEntity))
                .ReturnsAsync((Jogo)null);
            repositoryMock.Setup(m => m.Obter<Produtora>(It.IsAny<Guid>()))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act
            await service.Inserir(_testInputModel);

            // Assert
            repositoryMock.Verify(m => m.Inserir(It.IsAny<Jogo>()), Times.Once);
        }

        [Fact]
        public async Task Inserir_ShouldReturnViewModel_IfNoConflits()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(_testEntity))
                .ReturnsAsync((Jogo)null);
            repositoryMock.Setup(m => m.Obter<Produtora>(It.IsAny<Guid>()))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act
            var result = await service.Inserir(_testInputModel);

            // Assert
            result.ShouldBeOfType<JogoViewModel>();
        }

        [Fact]
        public async Task Inserir_ShouldThrowEntidadeJaCadastradaException_IfRepositoryReturnedConflitingEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Jogo>()))
                .ReturnsAsync(new Jogo());
            repositoryMock.Setup(m => m.Obter<Produtora>(It.IsAny<Guid>()))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeJaCadastradaException>
                (async () => await service.Inserir(_testInputModel));
        }

        [Fact]
        public async Task Atualizar_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            repositoryMock.Setup(m => m.Obter<Produtora>(It.IsAny<Guid>()))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act
            await service.Atualizar(_testGuid, _testInputModel);

            // Assert
            repositoryMock.Verify(m => m.Atualizar(_testEntity), Times.Once);
        }

        [Fact]
        public async Task Atualizar_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            repositoryMock.Setup(m => m.Obter<Produtora>(It.IsAny<Guid>()))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act
            var result = await service.Atualizar(_testGuid, _testInputModel);

            // Assert
            result.ShouldBeOfType<JogoViewModel>();
        }

        [Fact]
        public async Task Atualizar_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Jogo)null);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Jogo>()));
            var service = new JogoService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Atualizar(_testGuid, _testInputModel));
        }

        [Fact]
        public async Task Remover_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            var service = new JogoService(repositoryMock.Object);

            // Act
            await service.Remover(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Remover(_testGuid), Times.Once);
        }

        [Fact]
        public async Task Remover_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Jogo)null);
            var service = new JogoService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Remover(_testGuid));
        }

        [Fact]
        public async Task AtualizarValor_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testEntity);
            var service = new JogoService(repositoryMock.Object);

            // Act
            var result = await service.AtualizarValor(_testGuid, 70);

            // Assert
            repositoryMock.Verify(m => m.Obter(_testGuid), Times.Once);
            repositoryMock.Verify(m => m.Atualizar(_testEntity), Times.Once);
        }

        [Fact]
        public async Task AtualizarValor_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IJogoRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Jogo)null);
            var service = new JogoService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.AtualizarValor(_testGuid, 70));
        }
    }
}
