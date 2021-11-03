using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.Model.InputModel;
using ApiCatalogoJogos.Model.ViewModel;

namespace ApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _repository;

        public JogoService(IJogoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _repository.Obter(pagina, quantidade);

            return await ObterViewModels(jogos);
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _repository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            return await ObterViewModel(jogo);
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogoInput)
        {
            var jogos = await _repository.Obter(jogoInput.Nome, jogoInput.ProdutoraId);

            if (jogos.Count > 0)
            {
                var ex = new EntidadeJaCadastradaException("Jogo com mesmo nome já cadastrado para esta produtora");
                ex.Data.Add("JogoConflitante", jogos.First());
                throw ex;
            }

            var jogo = new Jogo()
            {
                Id = Guid.NewGuid(),
                Nome = jogoInput.Nome,
                ProdutoraId = jogoInput.ProdutoraId,
                Ano = jogoInput.Ano
            };

            await _repository.Inserir(jogo);

            return await ObterViewModel(jogo);
        }

        public async Task<JogoViewModel> Atualizar(Guid id, JogoInputModel jogoInput)
        {
            var jogo = await _repository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            jogo.Nome = jogoInput.Nome;
            jogo.ProdutoraId = jogoInput.ProdutoraId;
            jogo.Ano = jogoInput.Ano;

            await _repository.Atualizar(jogo);

            return await ObterViewModel(jogo);
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _repository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            await _repository.Remover(id);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        // Util
        private async Task<JogoViewModel> ObterViewModel(Jogo jogo)
        {
            return new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                ProdutoraId = jogo.ProdutoraId,
                Ano = jogo.Ano
            };
        }

        private async Task<List<JogoViewModel>> ObterViewModels(List<Jogo> jogos)
        {
            var list = new List<JogoViewModel>();

            foreach (var jogo in jogos)
            {
                list.Add(await ObterViewModel(jogo));
            }

            return list;
        }
    }
}
