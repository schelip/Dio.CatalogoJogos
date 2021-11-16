using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;

namespace Dio.CatalogoJogos.Api.Business.Services
{
    public class UsuarioService : ServiceBase<UsuarioInputModel, UsuarioViewModel, Usuario>, IUsuarioService
    {
        protected readonly new IUsuarioRepository _repository;
        protected readonly IJwtUtils _jwtUtils;

        public UsuarioService(IUsuarioRepository repository, IJwtUtils jwtUtils) : base(repository)
        {
            _repository = repository;
            _jwtUtils = jwtUtils;
        }

        public async Task<(string, UsuarioViewModel)> Autenticar(LoginInputModel inputModel)
        {
            var usuario = await _repository.Obter(inputModel.Email);

            if (usuario == null)
                throw new EntidadeNaoCadastradaException("Email inválido");

            if (!BCrypt.Net.BCrypt.Verify(inputModel.Senha, usuario.SenhaHash))
                throw new ModelInvalidoException("Senha inválida");

            var token = _jwtUtils.GerarJwtToken(usuario);

            return (token, await ObterViewModel(usuario));
        }

        public async Task<UsuarioViewModel> AtualizarFundos(Guid guid, float quant)
        {
            var usuario = await _repository.Obter(guid);

            if (usuario == null)
                throw new EntidadeNaoCadastradaException(guid);

            usuario.Fundos = quant;
            await _repository.Atualizar(usuario);
            return await ObterViewModel(usuario);
        }

        public async Task<UsuarioViewModel> AdicionarJogo(Guid idUsuario, Guid idJogo)
        {
            var usuario = await _repository.Obter(idUsuario);
            var jogo = await _repository.Obter<Jogo>(idJogo);

            if (usuario == null)
                throw new EntidadeNaoCadastradaException(idUsuario);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException(idJogo);

            if (usuario.UsuarioJogos.Any(uj => uj.JogoId == idJogo))
                throw new EntidadeJaCadastradaException("O usuário já possui esse jogo");

            if (jogo.Valor > usuario.Fundos)
                throw new FundosInsuficientesException(jogo.Valor - usuario.Fundos);

            await _repository.AdicionarJogo(usuario, jogo);
            await AtualizarFundos(idUsuario, usuario.Fundos - jogo.Valor);
            return await ObterViewModel(usuario);
        }

        protected override async Task<Usuario> ObterEntidade(Guid guid, UsuarioInputModel inputModel)
        {
            ValidarPermissao(inputModel.Permissao);

            var usuario = guid == Guid.Empty
                ? new Usuario()
                {
                    Id = Guid.NewGuid(),
                    UsuarioJogos = new List<UsuarioJogo>()
                }
                : await _repository.Obter(guid)
                ?? throw new EntidadeNaoCadastradaException(guid);

            usuario.Nome = inputModel.Nome;
            usuario.Email = inputModel.Email;
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(inputModel.Senha);
            usuario.Fundos = inputModel.Fundos;
            usuario.Permissao = inputModel.Permissao;

            return usuario;
        }

        protected override async Task<UsuarioViewModel> ObterViewModel(Usuario usuario)
        {
            return new UsuarioViewModel()
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Fundos = usuario.Fundos,
                Permissao = usuario.Permissao,
                Jogos = (await _repository.ObterJogos(usuario))
                    .Select(j => j.Id).ToList()
            };
        }

        // Util
        private void ValidarPermissao(string permissao)
        {
            var permissoes = typeof(PermissaoUsuario).GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Select(p => p.GetValue(null) as string);

            if (!permissoes.Contains(permissao))
                throw new ModelInvalidoException("A permissão informada não existe");
        }
    }
}
