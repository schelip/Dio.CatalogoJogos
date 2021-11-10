using System;
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
    public class ProdutoraService : ServiceBase<ProdutoraInputModel, ProdutoraViewModel, Produtora>, IProdutoraService
    {
        public ProdutoraService(IProdutoraRepository repository) : base(repository)
        {
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
                                            ?? throw new EntidadeNaoCadastradaException()
                                    : null;

            return produtora;
        }

        protected override async Task<ProdutoraViewModel> ObterViewModel(Produtora produtora)
        {

            var filhas = await _repository.Obter(("ProdutoraMae", produtora));
            var jogos = await _repository.Obter<Jogo>(("Produtora", produtora));
            foreach (var filha in filhas)
                jogos.AddRange(await _repository.Obter<Jogo>(("Produtora", filha)));

            return new ProdutoraViewModel()
            {
                Id = produtora.Id,
                Nome = produtora.Nome,
                ISOPais = produtora.ISOPais,
                ProdutoraMaeId = produtora.ProdutoraMae == null ? Guid.Empty : produtora.ProdutoraMae.Id,
                ProdutorasFilhas = filhas.Select(p => p.Id).ToList(),
                JogosProduzidos = jogos.Select(j => j.Id).ToList()
            };
        }

        protected override (string, object)[] ObterParametrosParaConflito(Produtora produtora)
        {
            var ps = new (string, object)[]
            {
                ("Nome", produtora.Nome)
            };

            if (produtora.ProdutoraMae != null)
                ps.Append(("ProdutoraMae", produtora.ProdutoraMae));

            return ps;
        }
    }
}
