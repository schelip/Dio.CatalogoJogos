using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class ProdutoraServiceTests
    {
        private readonly Guid _testGuid;
        private readonly Guid _maeGuid;
        private readonly string _validISO;
        private readonly string _invalidISO;
        private readonly Produtora _testProdutora;
        private readonly Produtora _testProdutoraSemMae;
        private readonly Produtora _testProdutoraMae;
        private readonly ProdutoraInputModel _testInputModel;
        private readonly ProdutoraInputModel _testInputModelSemMae;
        private readonly ProdutoraInputModel _invalidInputModel;
        private readonly List<Produtora> _produtoraList;
        private readonly List<Jogo> _jogoList;

        public ProdutoraServiceTests()
        {
            _testGuid = Guid.NewGuid();
            _maeGuid = Guid.NewGuid();
            _validISO = "US";
            _invalidISO = "xx";
            _testProdutoraMae = new Produtora()
            {
                Id = _maeGuid
            };
            _testProdutora = new Produtora()
            {
                Id = _testGuid,
                Nome = "ProdutoraTeste",
                ISOPais = _validISO,
                ProdutoraMae = _testProdutoraMae
            };
            _testProdutoraSemMae = new Produtora()
            {
                Id = _testGuid,
                Nome = "ProdutoraTeste",
                ISOPais = _validISO,
                ProdutoraMae = null
            };
            _testInputModel = new ProdutoraInputModel()
            {
                Nome = "ProdutoraTeste",
                ISOPais = _validISO,
                ProdutoraMaeId = _maeGuid
            };
            _testInputModelSemMae = new ProdutoraInputModel()
            {
                Nome = "ProdutoraTeste",
                ISOPais = _validISO,
                ProdutoraMaeId = null
            };
            _invalidInputModel = new ProdutoraInputModel()
            {
                ISOPais = _invalidISO
            };
            _produtoraList = new List<Produtora>()
            {
                new Produtora()
                {
                    Id = Guid.NewGuid()
                }
            };
            _jogoList = new List<Jogo>()
            {
                new Jogo()
                {
                    Id = Guid.NewGuid()
                }
            };
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Produtora>());
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Obter(1, 5);

            // Assert
            repositoryMock.Verify(m => m.Obter(1, 5), Times.Once);
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnViewModelList()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterFilhas(_testProdutora))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterJogos(_testProdutora))
                .ReturnsAsync(_jogoList);
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Obter(1, 5);

            // Assert
            result.ShouldBeOfType<List<ProdutoraViewModel>>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.ObterFilhas(_testProdutora))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterJogos(_testProdutora))
                .ReturnsAsync(new List<Jogo>());
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Obter(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.ObterFilhas(_testProdutora))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterJogos(_testProdutora))
                .ReturnsAsync(new List<Jogo>());
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Obter(_testGuid);

            // Assert
            result.ShouldBeOfType<ProdutoraViewModel>();
        }

        [Fact]
        public async Task ObterPorId_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Produtora)null);
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Obter(_testGuid));
        }

        [Fact]
        public async Task Inserir_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Produtora>()))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync(_testProdutoraMae);
            repositoryMock.Setup(m => m.ObterFilhas(It.IsAny<Produtora>()))
                .ReturnsAsync(_produtoraList);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Produtora>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Inserir(_testInputModel);

            // Assert
            repositoryMock.Verify(m => m.Inserir(It.IsAny<Produtora>()), Times.Once);
        }

        [Fact]
        public async Task Inserir_ShouldReturnViewModel_IfNoConflits()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Produtora>()))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync(_testProdutoraMae);
            repositoryMock.Setup(m => m.ObterFilhas(It.IsAny<Produtora>()))
                .ReturnsAsync(_produtoraList);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Produtora>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Inserir(_testInputModel);

            // Assert
            result.ShouldBeOfType<ProdutoraViewModel>();
        }

        [Fact]
        public async Task Inserir_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull_AndProdutoraMaeGuidIsNotEmpty()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Inserir(_testInputModel));
        }

        [Fact]
        public async Task Inserir_ReturnViewModel_IfRepositoryReturnedNull_AndProdutoraMaeIdIsEmpty()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutoraSemMae);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.ObterFilhas(It.IsAny<Produtora>()))
                .ReturnsAsync(_produtoraList);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Produtora>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Inserir(_testInputModelSemMae);

            // Assert
            result.ShouldBeOfType<ProdutoraViewModel>();
        }

        [Fact]
        public async Task Inserir_ShouldThrowEntidadeJaCadastradaException_IfRepositoryReturnedConflitingEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Produtora>()))
                .ReturnsAsync(new Produtora());
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync(_testProdutoraMae);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeJaCadastradaException>
                (async () => await service.Inserir(_testInputModel));
        }

        [Fact]
        public async Task Inserir_ShouldThrowModelInvalidoException_IfISOIsInvalid()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Inserir(_invalidInputModel));
        }

        [Fact]
        public async Task Atualizar_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync(_testProdutoraMae);
            repositoryMock.Setup(m => m.ObterFilhas(_testProdutora))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterJogos(_testProdutora))
                .ReturnsAsync(new List<Jogo>());
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Atualizar(_testGuid, _testInputModel);

            // Assert
            repositoryMock.Verify(m => m.Atualizar(_testProdutora), Times.Once);
        }

        [Fact]
        public async Task Atualizar_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync(_testProdutoraMae);
            repositoryMock.Setup(m => m.ObterFilhas(_testProdutora))
                .ReturnsAsync(new List<Produtora>());
            repositoryMock.Setup(m => m.ObterJogos(_testProdutora))
                .ReturnsAsync(new List<Jogo>());
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Atualizar(_testGuid, _testInputModel);

            // Assert
            result.ShouldBeOfType<ProdutoraViewModel>();
        }

        [Fact]
        public async Task Atualizar_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Atualizar(_testGuid, _testInputModel));
        }
        
        [Fact]
        public async Task Atualizar_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull_AndProdutoraMaeGuidIsNotEmpty()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Atualizar(_testGuid, _testInputModel));
        }

        [Fact]
        public async Task Atualizar_ReturnViewModel_IfRepositoryReturnedNull_AndProdutoraMaeIdIsEmpty()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutoraSemMae);
            repositoryMock.Setup(m => m.Obter(_maeGuid))
                .ReturnsAsync((Produtora)null);
            repositoryMock.Setup(m => m.ObterFilhas(It.IsAny<Produtora>()))
                .ReturnsAsync(_produtoraList);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Produtora>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Produtora>()));
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            var result = await service.Atualizar(_testGuid, _testInputModelSemMae);

            // Assert
            result.ShouldBeOfType<ProdutoraViewModel>();
        }

        [Fact]
        public async Task Atualizar_ShouldThrowModelInvalidoException_IfISOIsInvalid()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Atualizar(_testGuid, _invalidInputModel));
        }

        [Fact]
        public async Task Remover_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testProdutora);
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Remover(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Remover(_testGuid), Times.Once);
        }

        [Fact]
        public async Task Remover_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Produtora)null);
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Remover(_testGuid));
        }

        [Fact]
        public async Task ObterPorPais_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(new List<Produtora>());
            var service = new ProdutoraService(repositoryMock.Object);

            // Act
            await service.Obter(_validISO);

            // Assert
            repositoryMock.Verify(m => m.Obter(_validISO), Times.Once);
        }

        [Fact]
        public async Task ObterPorPais_ShouldThrowModelInvalidoException_IfISOIsInvalid()
        {
            // Arrange
            var repositoryMock = new Mock<IProdutoraRepository>();
            var service = new ProdutoraService(repositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Obter(_invalidISO));
        }
    }
}
