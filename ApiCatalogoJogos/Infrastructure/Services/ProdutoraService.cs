using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Infrastructure.Services
{
    public class ProdutoraService : IProdutoraService
    {
        private readonly IProdutoraRepository _repository;

        public ProdutoraService(IProdutoraRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProdutoraViewModel>> Obter(int pagina, int quantidade)
        {
            var produtoras = await _repository.Obter(pagina, quantidade);

            return await ObterViewModels(produtoras);
        }

        public async Task<List<ProdutoraViewModel>> Obter(string isoPais)
        {
            if (!ValidaPais(isoPais))
                throw new PaisInexistenteException("País não encontrado");

            var produtoras = await _repository.Obter(isoPais);

            return await ObterViewModels(produtoras);
        }

        public async Task<ProdutoraViewModel> Obter(Guid id)
        {
            var produtora = await _repository.Obter(id);

            if (produtora == null)
                throw new EntidadeNaoCadastradaException("Produtora não cadastrada");

            return await ObterViewModel(produtora);
        }

        public async Task<ProdutoraViewModel> Inserir(ProdutoraInputModel produtoraInput)
        {
            if (!ValidaPais(produtoraInput.ISOPais))
                throw new PaisInexistenteException("País não encontrado");

            var produtoraConflitante = await _repository.Obter(produtoraInput.Nome, produtoraInput.ISOPais);

            if (produtoraConflitante != null)
            {
                var ex = new EntidadeJaCadastradaException("Produtora com mesmo nome e país de origem já cadastrada");
                ex.Data.Add("IdProdutoraConflitante", produtoraConflitante.Id);
                throw ex;
            }

            var produtora = new Produtora()
            {
                Id = Guid.NewGuid(),
                Nome = produtoraInput.Nome,
                ISOPais = produtoraInput.ISOPais,
                ProdutoraMae = produtoraInput.ProdutoraMaeId.HasValue
                    ? await _repository.Obter(produtoraInput.ProdutoraMaeId.Value)
                    ?? throw new EntidadeNaoCadastradaException("Produtora mãe com id informado não encontrada")
                    : null
            };

            await _repository.Inserir(produtora);

            return await ObterViewModel(produtora);
        }

        public async Task<ProdutoraViewModel> Atualizar(Guid id, ProdutoraInputModel produtoraInput)
        {
            var produtora = await _repository.Obter(id);

            if (produtora == null)
                throw new EntidadeNaoCadastradaException("Entidade não cadastrada");

            if (!ValidaPais(produtoraInput.ISOPais))
                throw new PaisInexistenteException("País não encontrado");

            produtora.Nome = produtoraInput.Nome;
            produtora.ISOPais = produtoraInput.ISOPais;
            produtora.ProdutoraMae = produtoraInput.ProdutoraMaeId.HasValue
                ? await _repository.Obter(produtoraInput.ProdutoraMaeId.Value)
                ?? throw new EntidadeNaoCadastradaException()
                : null;

            await _repository.Atualizar(produtora);

            return await ObterViewModel(produtora);
        }

        public async Task<ProdutoraViewModel> Atualizar(Guid idMae, Guid idFilha)
        {
            var mae = await _repository.Obter(idMae);
            var filha = await _repository.Obter(idFilha);

            if (mae == null)
                throw new EntidadeNaoCadastradaException("Produtora mãe não cadastrada");

            if (filha == null)
                throw new EntidadeNaoCadastradaException("Produtora filha não cadastrada");

            if ((await _repository.ObterFilhas(mae.Id)).Contains(filha))
                throw new EntidadeJaCadastradaException("Produtoras já estão cadastradas como mãe e filha");

            var produtora = await _repository.Atualizar(mae, filha);

            return await ObterViewModel(produtora);
        }

        public async Task Remover(Guid id)
        {
            var produtora = await _repository.Obter(id);

            if (produtora == null)
                throw new EntidadeNaoCadastradaException("Produtora não cadastrada");

            await _repository.Remover(id);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        // Util
        private async Task<ProdutoraViewModel> ObterViewModel(Produtora produtora)
        {
            var filhas = await _repository.ObterFilhas(produtora.Id);

            return new ProdutoraViewModel()
            {
                Id = produtora.Id,
                Nome = produtora.Nome,
                ISOPais = produtora.ISOPais,
                ProdutoraMaeId = produtora.ProdutoraMae == null ? Guid.Empty : produtora.ProdutoraMae.Id,
                ProdutorasFilhas = filhas.Select(f => f.Id).ToList(),
                JogosProduzidos = (await _repository.ObterJogos(produtora.Id)).Select(j => j.Id).ToList()
            };
        }

        private async Task<List<ProdutoraViewModel>> ObterViewModels(List<Produtora> produtoras)
        {
            List<ProdutoraViewModel> list = new List<ProdutoraViewModel>();

            foreach (var produtora in produtoras)
            {
                list.Add(await ObterViewModel(produtora));
            }

            return list;
        }

        private List<JogoViewModel> ObterViewModels(List<Jogo> jogos)
        {
            List<JogoViewModel> list = new List<JogoViewModel>();

            foreach (var jogo in jogos)
            {
                list.Add(new JogoViewModel()
                {
                    Id = jogo.Id,
                    Nome = jogo.Nome,
                    ProdutoraId = jogo.ProdutoraId,
                    Ano = jogo.Ano
                });
            }

            return list;
        }

        private static bool ValidaPais(string isoPais)
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.LCID))
                .Any(ri => ri.TwoLetterISORegionName == isoPais.ToUpper());
        }
    }
}
