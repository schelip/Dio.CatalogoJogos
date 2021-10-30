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
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Ano = jogo.Ano
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            return new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Ano = jogo.Ano
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogoInput)
        {
            var jogos = await _jogoRepository.Obter(jogoInput.Nome, jogoInput.Produtora);

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
                Produtora = jogoInput.Produtora,
                Ano = jogoInput.Ano
            };

            await _jogoRepository.Inserir(jogo);

            return new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Ano = jogo.Ano
            };
        }

        public async Task<JogoViewModel> Atualizar(Guid id, JogoInputModel jogoInput)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            jogo.Nome = jogoInput.Nome;
            jogo.Produtora = jogoInput.Produtora;
            jogo.Ano = jogoInput.Ano;

            await _jogoRepository.Atualizar(jogo);

            return new JogoViewModel()
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Ano = jogo.Ano
            };
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new EntidadeNaoCadastradaException("Jogo não cadastrado");

            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
