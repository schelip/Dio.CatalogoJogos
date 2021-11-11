﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Infrastructure.Services
{
    public class ProdutoraService : ServiceBase<ProdutoraInputModel, ProdutoraViewModel, Produtora>, IProdutoraService
    {
        private readonly new IProdutoraRepository _repository;

        public ProdutoraService(IProdutoraRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<ProdutoraViewModel>> Obter(string ISOPais)
        {
            return await ObterViewModels(await _repository.Obter(ISOPais));
        }

        protected override async Task<Produtora> ObterEntidade(Guid guid, ProdutoraInputModel inputModel)
        {
            var produtora = guid == Guid.Empty
                                            ? new Produtora() { Id = Guid.NewGuid() }
                                            : await _repository.Obter(guid);

            produtora.Nome = inputModel.Nome;
            produtora.ISOPais = inputModel.ISOPais.ToUpper();
            produtora.ProdutoraMae = inputModel.ProdutoraMaeId.HasValue
                                    ? await _repository.Obter(inputModel.ProdutoraMaeId.Value)
                                            ?? throw new EntidadeNaoCadastradaException(inputModel.ProdutoraMaeId.Value)
                                    : null;

            return produtora;
        }

        protected override async Task<ProdutoraViewModel> ObterViewModel(Produtora produtora)
        {
            return new ProdutoraViewModel()
            {
                Id = produtora.Id,
                Nome = produtora.Nome,
                ISOPais = produtora.ISOPais,
                ProdutoraMaeId = produtora.ProdutoraMae == null ? Guid.Empty : produtora.ProdutoraMae.Id,
                ProdutorasFilhas = (await _repository.ObterFilhas(produtora)).Select(p => p.Id).ToList(),
                JogosProduzidos = (await _repository.ObterJogos(produtora)).Select(p => p.Id).ToList()
            };
        }
    }
}
