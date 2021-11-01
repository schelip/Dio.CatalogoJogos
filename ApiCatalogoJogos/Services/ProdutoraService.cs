using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.Model.InputModel;
using ApiCatalogoJogos.Model.ViewModel;

namespace ApiCatalogoJogos.Services
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

        public async Task<List<ProdutoraViewModel>> ObterFilhas(Guid id)
        {
            var produtora = await _repository.Obter(id);

            if (produtora == null)
                throw new EntidadeNaoCadastradaException("Produtora não cadastrada");

            return await ObterViewModels(produtora.ProdutorasFilhas);
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
            if (!ValidaPais(produtoraInput.isoPais))
                throw new PaisInexistenteException("País não encontrado");

            var produtoraConflitante = await _repository.Obter(produtoraInput.Nome, produtoraInput.isoPais);

            if (produtoraConflitante != null)
            {
                var ex = new EntidadeJaCadastradaException("Entidade com mesmo nome e país de origem já cadastrada");
                ex.Data.Add("ProdutoraConflitante", produtoraConflitante);
                throw ex;
            }

            var produtora = new Produtora();

            await _repository.Inserir(produtora);

            return await ObterViewModel(produtora);
        }

        public async Task<ProdutoraViewModel> Atualizar(Guid id, ProdutoraInputModel produtoraInput)
        {
            var produtora = await _repository.Obter(id);

            if (produtora == null)
                throw new EntidadeNaoCadastradaException("Entidade não cadastrada");

            if (!ValidaPais(produtoraInput.isoPais))
                throw new PaisInexistenteException("País não encontrado");

            produtora.Nome = produtoraInput.Nome;
            produtora.isoPais = produtoraInput.isoPais;
            produtora.ProdutoraMae = await _repository.Obter(produtoraInput.IdProdutoraMae);

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

            if (mae.ProdutorasFilhas.Contains(filha))
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
            return new ProdutoraViewModel()
            {
                Id = produtora.Id,
                Nome = produtora.Nome,
                isoPais = produtora.isoPais,
                ProdutoraMae = await Obter(produtora.ProdutoraMae.Id),
                ProdutorasFilhas = await ObterFilhas(produtora.Id)
            };
        }

        private async Task<List<ProdutoraViewModel>> ObterViewModels(List<Produtora> produtoras)
        {
            List<ProdutoraViewModel> list = new List<ProdutoraViewModel>();

            foreach (var produtora in produtoras)
            {
                list.Add(new ProdutoraViewModel()
                {
                    Id = produtora.Id,
                    Nome = produtora.Nome,
                    isoPais = produtora.isoPais,
                    ProdutoraMae = await Obter(produtora.ProdutoraMae.Id),
                    ProdutorasFilhas = await ObterFilhas(produtora.Id)
                });
            }

            return list;
        }

        private static bool ValidaPais(string isoPais)
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.LCID))
                .Any(ri => ri.Name.Equals(isoPais, StringComparison.InvariantCulture));
        }
    }
}
