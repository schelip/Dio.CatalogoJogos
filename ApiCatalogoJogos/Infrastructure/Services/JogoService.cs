using System;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Infrastructure.Services
{
    public class JogoService : ServiceBase<JogoInputModel, JogoViewModel, Jogo>, IJogoService
    {
        public JogoService(IJogoRepository repository) : base(repository)
        {
        }

        protected override async Task<Jogo> ObterEntidade(Guid guid, JogoInputModel inputModel)
        {
            var jogo = guid == Guid.Empty
                               ? new Jogo() { Id = Guid.NewGuid() }
                               : await _repository.Obter(guid);

            jogo.Nome = inputModel.Nome;
            jogo.Ano = inputModel.Ano;
            jogo.ProdutoraId = inputModel.ProdutoraId;

            return jogo;
        }

        protected override Task<JogoViewModel> ObterViewModel(Jogo jogo)
        {
            return Task.FromResult(new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Ano = jogo.Ano,
                ProdutoraId = jogo.ProdutoraId
            });
        }

        protected override (string, object)[] ObterParametrosParaConflito(Jogo jogo)
        {
            var prms = new (string, object)[]
            {
                ("Nome", jogo.Nome),
                ("ProdutoraId", jogo.ProdutoraId)
            };

            return prms;
        }
    }
}
