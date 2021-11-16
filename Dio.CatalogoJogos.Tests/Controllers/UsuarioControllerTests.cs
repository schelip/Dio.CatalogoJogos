using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Controllers.v1;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Dio.CatalogoJogos.Tests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Guid _testGuid;
        private readonly Guid _adminGuid;
        private readonly Guid _otherGuid;
        private readonly Guid _jogoGuid;
        private readonly Usuario _testUsuario;
        private readonly Usuario _testAdmin;
        private readonly UsuarioInputModel _testInputModel;
        private readonly UsuarioViewModel _testViewModel;
        private readonly List<UsuarioViewModel> _emptyList;
        private readonly List<UsuarioViewModel> _list;

        public UsuarioControllerTests()
        {
            _testGuid = Guid.NewGuid();
            _adminGuid = Guid.NewGuid();
            _otherGuid = Guid.NewGuid();
            _jogoGuid = Guid.NewGuid();
            _testUsuario = new Usuario()
            {
                Id = _testGuid,
                Email = "user@example.com",
                Permissao = PermissaoUsuario.Usuario
            };
            _testAdmin = new Usuario()
            {
                Id = _adminGuid,
                Email = "admin@example.com",
                Permissao = PermissaoUsuario.Admin
            };
            _testInputModel = new UsuarioInputModel();
            _testViewModel = new UsuarioViewModel();
            _emptyList = new List<UsuarioViewModel>();
            _list = new List<UsuarioViewModel>() { _testViewModel };
        }

        [Fact]
        public async Task ObterPaginado_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            serviceMock.Verify(m => m.Obter(1, 5), Times.Once());
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnOk_IfServiceReturnedModels()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_list);
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterPaginado_ShouldReturnNoContent_IfServiceReturnsEmptyList()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_emptyList);
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Obter(1, 5);

            // Assert
            result.Result.ShouldBeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ObterPorId_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Obter(_testGuid), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnForbid_IfUserIdIsDifferentFromUserLoggedIn_AndUserIsNotAdmin()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);
            FakeUserLogin(controller);

            // Act
            var result = await controller.Obter(_otherGuid);

            // Assert
            result.Result.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ObterPorId_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Obter(It.IsAny<Guid>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Obter(_testGuid);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Inserir_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_testInputModel);

            // Assert
            serviceMock.Verify(m => m.Inserir(_testInputModel), Times.Once());
        }

        [Fact]
        public async Task Inserir_ShouldReturnCreated_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Inserir(It.IsAny<UsuarioInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Inserir(_testInputModel);

            // Assert
            result.Result.ShouldBeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Atualizar_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, _testInputModel);

            // Assert
            serviceMock.Verify(m => m.Atualizar(_testGuid, _testInputModel), Times.Once());
        }

        [Fact]
        public async Task Atualizar_ShouldReturnForbid_IfUserIdIsDifferentFromUserLoggedIn_AndUserIsNotAdmin()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeUserLogin(controller);

            // Act
            var result = await controller.Atualizar(_otherGuid, _testInputModel);

            // Assert
            result.Result.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Atualizar_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Atualizar(It.IsAny<Guid>(), It.IsAny<UsuarioInputModel>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, _testInputModel);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AtualizarFundos_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            float quant = 100;
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, quant);

            // Assert
            serviceMock.Verify(m => m.AtualizarFundos(_testGuid, quant), Times.Once());
        }

        [Fact]
        public async Task AtualizarFundos_ShouldReturnForbid_IfUserIdIsDifferentFromUserLoggedIn_AndUserIsNotAdmin()
        {
            // Arrange
            float quant = 100;
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeUserLogin(controller);

            // Act
            var result = await controller.Atualizar(_otherGuid, quant);

            // Assert
            result.Result.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task AtualizarFundos_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            float quant = 100;
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.AtualizarFundos(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, quant);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AtualizarJogos_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, _jogoGuid);

            // Assert
            serviceMock.Verify(m => m.AdicionarJogo(_testGuid, _jogoGuid), Times.Once());
        }

        [Fact]
        public async Task AtualizarJogos_ShouldReturnForbid_IfUserIdIsDifferentFromUserLoggedIn_AndUserIsNotAdmin()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeUserLogin(controller);

            // Act
            var result = await controller.Atualizar(_otherGuid, _jogoGuid);

            // Assert
            result.Result.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task AtualizarJogos_ShouldReturnOk_IfServiceReturnedModel()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.AdicionarJogo(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(_testViewModel);
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Atualizar(_testGuid, _testGuid);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Remover_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            serviceMock.Verify(m => m.Remover(_testGuid), Times.Once());
        }

        [Fact]
        public async Task Remover_ShouldReturnForbid_IfUserIdIsDifferentFromUserLoggedIn_AndUserIsNotAdmin()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            var controller = new UsuarioController(serviceMock.Object);
            FakeUserLogin(controller);

            // Act
            var result = await controller.Remover(_otherGuid);

            // Assert
            result.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Remover_ShouldReturnOk_IfServiceSucceeded()
        {
            // Arrange
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Remover(It.IsAny<Guid>()));
            var controller = new UsuarioController(serviceMock.Object);
            FakeAdminLogin(controller);

            // Act
            var result = await controller.Remover(_testGuid);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Autenticar_ShouldCallServiceWithCorrectParameters()
        {
            // Arrange
            var inputModel = new LoginInputModel();
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Autenticar(It.IsAny<LoginInputModel>()));
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            await controller.Autenticar(inputModel);

            // Assert
            serviceMock.Verify(m => m.Autenticar(inputModel), Times.Once);
        }

        [Fact]
        public async Task Autenticar_ShouldReturnOk_AndReturnToken_AndReturnViewModel()
        {
            // Arrange
            var inputModel = new LoginInputModel();
            var serviceMock = new Mock<IUsuarioService>();
            serviceMock.Setup(m => m.Autenticar(It.IsAny<LoginInputModel>()))
                .ReturnsAsync(("tokenTest", _testViewModel));
            var controller = new UsuarioController(serviceMock.Object);

            // Act
            var result = await controller.Autenticar(inputModel);

            // Assert
            result.Result.ShouldBeOfType<OkObjectResult>();
            dynamic data = (result.Result as OkObjectResult).Value;
            Assert.IsType<string>(data.token);
            Assert.IsType<UsuarioViewModel>(data.usuario);
        }

        // Util
        private void FakeAdminLogin(UsuarioController controller)
        {
            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _testAdmin.Id.ToString()),
                new Claim(ClaimTypes.Email, _testAdmin.Email),
                new Claim(ClaimTypes.Role, _testAdmin.Permissao)
            }, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identity);
            principalMock.Setup(x => x.IsInRole("Administrador")).Returns(true);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User).Returns(claimsPrincipal);

            controller.ControllerContext.HttpContext = httpContextMock.Object;
        }

        private void FakeUserLogin(UsuarioController controller)
        {
            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _testUsuario.Id.ToString()),
                new Claim(ClaimTypes.Email, _testUsuario.Email),
                new Claim(ClaimTypes.Role, _testUsuario.Permissao)
            }, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(x => x.Identity).Returns(identity);
            principalMock.Setup(x => x.IsInRole("Administrador")).Returns(false);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User).Returns(claimsPrincipal);

            controller.ControllerContext.HttpContext = httpContextMock.Object;
        }
    }
}
