using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Business.Services;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Moq;
using Shouldly;
using Xunit;

namespace Dio.CatalogoJogos.Test.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IJwtUtils> _jwtMock;
        private readonly Guid _testGuid;
        private readonly Guid _jogoPossuidoGuid;
        private readonly Guid _jogoNovoGuid;
        private readonly Usuario _testUsuario;
        private readonly UsuarioInputModel _validInputModel;
        private readonly UsuarioInputModel _invalidInputModel;
        private readonly Jogo _jogoPossuido;
        private readonly Jogo _jogoNovo;
        private readonly List<Jogo> _jogoList;

        public UsuarioServiceTests()
        {
            _testGuid = Guid.NewGuid();
            _jogoPossuidoGuid = Guid.NewGuid();
            _jogoNovoGuid = Guid.NewGuid();
            var usuarioJogo = new UsuarioJogo()
            {
                Id = Guid.NewGuid(),
                UsuarioId = _testGuid,
                JogoId = _jogoPossuidoGuid
            };
            _testUsuario = new Usuario()
            {
                Id = _testGuid,
                Nome = "UsuarioTest",
                Email = "user@example.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("string"),
                Fundos = 100f,
                UsuarioJogos = new List<UsuarioJogo>() { usuarioJogo },
                Permissao = "Usuario"
            };
            _validInputModel = new UsuarioInputModel()
            {
                Nome = "UsuarioTest",
                Email = "user@example.com",
                Senha = "string",
                Fundos = 100f,
                Permissao = "Usuario"
            };
            _invalidInputModel = new UsuarioInputModel()
            {
                Nome = "UsuarioTest",
                Email = "user@example.com",
                Senha = "string",
                Fundos = 100f,
                Permissao = "PermissaoInvalida"
            };
            _jogoPossuido = new Jogo()
            {
                Id = _jogoPossuidoGuid,
                Valor = 20f,
                ProdutoraId = Guid.NewGuid(),
                UsuarioJogos = new List<UsuarioJogo>() { usuarioJogo }
            };
            _jogoNovo = new Jogo()
            {
                Id = _jogoNovoGuid,
                Valor = 20f,
                ProdutoraId = Guid.NewGuid(),
                UsuarioJogos = null
            };
            _jogoList = new List<Jogo>() { _jogoPossuido };
            _jwtMock = new Mock<IJwtUtils>();
            _jwtMock.Setup(m => m.GerarJwtToken(_testUsuario))
                .Returns("tokenTesttokenTesttokenTesttokenTest");
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Usuario>());
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Obter(1, 5);

            // Assert
            repositoryMock.Verify(m => m.Obter(1, 5), Times.Once);
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnViewModelList()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Usuario>());
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.Obter(1, 5);

            // Assert
            result.ShouldBeOfType<List<UsuarioViewModel>>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Obter(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.Obter(_testGuid);

            // Assert
            result.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task ObterPorId_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Usuario)null);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Obter(_testGuid));
        }

        [Fact]
        public async Task Inserir_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Usuario>()))
                .ReturnsAsync((Usuario)null);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Usuario>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Inserir(_validInputModel);

            // Assert
            repositoryMock.Verify(m => m.Inserir(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task Inserir_ShouldReturnViewModel_IfNoConflits()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Usuario>()))
                .ReturnsAsync((Usuario)null);
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(It.IsAny<Usuario>()))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.Inserir(_validInputModel);

            // Assert
            result.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task Inserir_ShouldThrowEntidadeJaCadastradaException_IfRepositoryReturnedConflitingEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.ObterConflitante(It.IsAny<Usuario>()))
                .ReturnsAsync(new Usuario());
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Inserir(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeJaCadastradaException>
                (async () => await service.Inserir(_validInputModel));
        }

        [Fact]
        public async Task Inserir_ShouldThrowModelInvalidoException_IfPermissaoIsInvalid()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Inserir(_invalidInputModel));
        }

        [Fact]
        public async Task Atualizar_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Atualizar(_testGuid, _validInputModel);

            // Assert
            repositoryMock.Verify(m => m.Atualizar(_testUsuario), Times.Once);
        }

        [Fact]
        public async Task Atualizar_ShouldReturnViewModel_IfRepositoryReturnedEntity()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.Atualizar(_testGuid, _validInputModel);

            // Assert
            result.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task Atualizar_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Usuario)null);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Atualizar(_testGuid, _validInputModel));
        }

        [Fact]
        public async Task Atualizar_ShouldThrowModelInvalidoException_IfPermissaoIsInvalid()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Atualizar(_testGuid, _invalidInputModel));
        }

        [Fact]
        public async Task Remover_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Remover(_testGuid);

            // Assert
            repositoryMock.Verify(m => m.Remover(_testGuid), Times.Once);
        }

        [Fact]
        public async Task Remover_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Usuario)null);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Remover(_testGuid));
        }
    
        [Fact]
        public async Task Autenticar_ShouldCallJwtUtilWithCorretParameters()
        {
            // Arrange
            var loginModel = new LoginInputModel()
            {
                Email = "user@example.com",
                Senha = "string"
            };
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.Autenticar(loginModel);

            // Assert
            _jwtMock.Verify(m => m.GerarJwtToken(_testUsuario), Times.Once);
        }

        [Fact]
        public async Task Autenticar_ShouldReturnToken_AndReturnViewModel_IfLoginIsValid()
        {
            // Arrange
            var validLoginModel = new LoginInputModel()
            {
                Email = "user@example.com",
                Senha = "string"
            };
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.Autenticar(validLoginModel);

            // Assert
            result.Item1.ShouldBeOfType<string>();
            result.Item2.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task Autenticar_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var invalidLoginModel = new LoginInputModel()
            {
                Email = "invalido@example.com",
                Senha = "string"
            };
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync((Usuario)null);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.Autenticar(invalidLoginModel));
        }

        [Fact]
        public async Task Autenticar_ShouldThrowModelInvalidoException_IfPasswordValidationFailed()
        {
            // Arrange
            var invalidLoginModel = new LoginInputModel()
            {
                Email = "user@example.com",
                Senha = "invalida"
            };
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(It.IsAny<string>()))
                .ReturnsAsync(_testUsuario);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ModelInvalidoException>
                (async () => await service.Autenticar(invalidLoginModel));
        }

        [Fact]
        public async Task AtualizarFundos_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var quant = 50f;
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.AtualizarFundos(_testGuid, quant);

            // Assert
            repositoryMock.Verify(m => m.Atualizar(_testUsuario), Times.Once);
        }

        [Fact]
        public async Task AtualizarFundos_ShouldReturnViewModel()
        {
            // Arrange
            var quant = 50f;
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.AtualizarFundos(_testGuid, quant);

            // Assert
            result.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task AtualizarFundos__ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull()
        {
            // Arrange
            var quant = 50f;
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Usuario)null);
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.AtualizarFundos(_testGuid, quant));
        }

        [Fact]
        public async Task AdicionarJogo_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoNovoGuid))
                .ReturnsAsync(_jogoNovo);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.AdicionarJogo(It.IsAny<Usuario>(), It.IsAny<Jogo>()));
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            await service.AdicionarJogo(_testGuid, _jogoNovoGuid);

            // Assert
            repositoryMock.Verify(m => m.AdicionarJogo(_testUsuario, _jogoNovo), Times.Once);
            repositoryMock.Verify(m => m.Atualizar(_testUsuario), Times.Once);
        }

        [Fact]
        public async Task AdicionarJogo_ShouldReturnViewModel()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoNovoGuid))
                .ReturnsAsync(_jogoNovo);
            repositoryMock.Setup(m => m.ObterJogos(_testUsuario))
                .ReturnsAsync(_jogoList);
            repositoryMock.Setup(m => m.AdicionarJogo(It.IsAny<Usuario>(), It.IsAny<Jogo>()));
            repositoryMock.Setup(m => m.Atualizar(It.IsAny<Usuario>()));
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act
            var result = await service.AdicionarJogo(_testGuid, _jogoNovoGuid);

            // Assert
            result.ShouldBeOfType<UsuarioViewModel>();
        }

        [Fact]
        public async Task AdicionarJogo_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull_TryingToGetUsuario()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync((Usuario)null);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoNovoGuid))
                .ReturnsAsync(_jogoNovo);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.AdicionarJogo(_testGuid, _jogoNovoGuid));
        }

        [Fact]
        public async Task AdicionarJogo_ShouldThrowEntidadeNaoCadastradaException_IfRepositoryReturnedNull_TryingToGetJogo()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoNovoGuid))
                .ReturnsAsync((Jogo)null);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeNaoCadastradaException>
                (async () => await service.AdicionarJogo(_testGuid, _jogoNovoGuid));
        }

        [Fact]
        public async Task AdicionarJogo_ShouldThrowEntidadeJaCadastradaException_IfUsuarioAlreadyHasJogo()
        {
            // Arrange
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoPossuidoGuid))
                .ReturnsAsync(_jogoPossuido);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<EntidadeJaCadastradaException>
                (async () => await service.AdicionarJogo(_testGuid, _jogoPossuidoGuid));
        }

        [Fact]
        public async Task AdicionarJogo_ShouldThrowFundosInsuficientesException_IfPriceIsTooGreat()
        {
            // Arrange
            var jogo = _jogoNovo;
            jogo.Valor = 9999f;
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(m => m.Obter(_testGuid))
                .ReturnsAsync(_testUsuario);
            repositoryMock.Setup(m => m.Obter<Jogo>(_jogoNovoGuid))
                .ReturnsAsync(jogo);
            var service = new UsuarioService(repositoryMock.Object, _jwtMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<FundosInsuficientesException>
                (async () => await service.AdicionarJogo(_testGuid, _jogoNovoGuid));
        }
    }
}
