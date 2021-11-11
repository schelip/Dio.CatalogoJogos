﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Infrastructure.Services
{
    public class UsuarioService : ServiceBase<UsuarioInputModel, UsuarioViewModel, Usuario>, IUsuarioService
    {
        protected readonly new IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository) : base(repository)
        {
            _repository = repository;
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
            usuario.Senha = inputModel.Senha;
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
                Senha = usuario.Senha,
                Fundos = usuario.Fundos,
                Permissao = usuario.Permissao,
                Jogos = (await _repository.ObterJogos(usuario))
                    .Select(j => j.Id).ToList()
            };
        }
    }
}
