using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Authorization;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace ApiCatalogoJogos.Infrastructure.Services
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

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(inputModel.Senha, usuario.SenhaHash))
                throw new AutenticacaoException("Email ou Senha inválidos");

            var token = _jwtUtils.GerarJwtToken(usuario);

            return (token, await ObterViewModel(usuario));
        }

        public async Task<UsuarioViewModel> Obter(string email)
        {
            var usuario = await _repository.Obter(email);

            if (usuario == null)
                throw new EntidadeNaoCadastradaException("Usuário de email "+ email + "não encontrado");

            return await ObterViewModel(usuario);
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

            if (jogo.Valor > usuario.Fundos)
                throw new FundosInsuficientesException();

            await _repository.AdicionarJogo(usuario, jogo);
            await AtualizarFundos(idUsuario, usuario.Fundos - jogo.Valor);
            return await ObterViewModel(usuario);
        }

        protected override async Task<Usuario> ObterEntidade(Guid guid, UsuarioInputModel inputModel)
        {
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
                Senha = usuario.SenhaHash,
                Fundos = usuario.Fundos,
                Permissao = usuario.Permissao,
                Jogos = (await _repository.ObterJogos(usuario))
                    .Select(j => j.Id).ToList()
            };
        }
    }
}
