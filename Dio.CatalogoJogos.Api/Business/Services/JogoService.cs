using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;

namespace Dio.CatalogoJogos.Api.Business.Services
{
    public class JogoService : ServiceBase<JogoInputModel, JogoViewModel, Jogo>, IJogoService
    {
        public JogoService(IJogoRepository repository) : base(repository)
        {
        }

        public async Task<JogoViewModel> AtualizarValor(Guid id, float valor)
        {
            var jogo = await _repository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException(id);

            jogo.Valor = valor;
            await _repository.Atualizar(jogo);
            return await ObterViewModel(jogo);
        }

        protected override async Task<Jogo> ObterEntidade(Guid guid, JogoInputModel inputModel)
        {
            var jogo = guid == Guid.Empty
                ? new Jogo() 
                {
                    Id = Guid.NewGuid(),
                    UsuarioJogos = new List<UsuarioJogo>()
                }
                : await _repository.Obter(guid)
                ?? throw new EntidadeNaoCadastradaException(guid);

            jogo.Nome = inputModel.Nome;
            jogo.Ano = inputModel.Ano;
            jogo.ProdutoraId = inputModel.ProdutoraId;
            jogo.Produtora = await _repository.Obter<Produtora>(inputModel.ProdutoraId)
                ?? throw new EntidadeNaoCadastradaException(inputModel.ProdutoraId);
            jogo.Valor = inputModel.Valor;

            return jogo;
        }

        protected override Task<JogoViewModel> ObterViewModel(Jogo jogo)
        {
            return Task.FromResult(new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Ano = jogo.Ano,
                ProdutoraId = jogo.ProdutoraId,
                Valor = jogo.Valor
            });
        }
    }
}
