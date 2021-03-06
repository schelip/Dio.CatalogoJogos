using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;

namespace Dio.CatalogoJogos.Api.Business.Services
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
            ValidaPais(ISOPais);

            return await ObterViewModels(await _repository.Obter(ISOPais));
        }

        protected override async Task<Produtora> ObterEntidade(Guid guid, ProdutoraInputModel inputModel)
        {
            ValidaPais(inputModel.ISOPais);

            var produtora = guid == Guid.Empty
                ? new Produtora() { Id = Guid.NewGuid() }
                : await _repository.Obter(guid)
                ?? throw new EntidadeNaoCadastradaException(guid);

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
            var pmaeid = produtora.ProdutoraMae == null ? Guid.Empty : produtora.ProdutoraMae.Id;
            var filhas = (await _repository.ObterFilhas(produtora));
            var idFilhas = filhas.Select(p => p.Id).ToList();
            var jogos = (await _repository.ObterJogos(produtora)).Select(p => p.Id).ToList();
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

        private void ValidaPais(string ISOPais)
        {
            var valido = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.LCID))
                .Any(ri => ri.TwoLetterISORegionName == ISOPais.ToUpper());

            if (!valido)
                throw new ModelInvalidoException($"O ISO {ISOPais} não corresponde a um país");
        }
    }
}
