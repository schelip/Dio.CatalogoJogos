﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
{
    /// <summary>
    /// Contrato para classe que gerencia operações no repositório utilizando Models
    /// </summary>
    public interface IJogoService : IDisposable
    {
        /// <summary>
        /// Obtém uma quantidade definida de ViewModels de jogos a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de jogos por página</param>
        /// <returns>Lista de jogos obtida</returns>
        Task<List<JogoViewModel>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém ViewModel de jogo do respositório pelo id
        /// </summary>
        /// <param name="id">Id do jogo a ser obtido</param>
        /// <returns>Jogo obtido</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe um jogo cadastrado com esse id</exception>
        Task<JogoViewModel> Obter(Guid id);
        /// <summary>
        /// Insere jogo no repositorio a partir de uma InputModel
        /// </summary>
        /// <param name="jogoInput">Jogo a ser inserido</param>
        /// <returns>Jogo inserido</returns>
        /// <exception cref="EntidadeJaCadastradaException">Se jogo com mesmo nome e produtora já está cadastrado</exception>
        Task<JogoViewModel> Inserir(JogoInputModel jogoInput);
        /// <summary>
        /// Atualiza todos os dados de um jogo cadastrado para os dados de uma InputModel
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado</param>
        /// <param name="jogoInput">Novos dados para o jogo</param>
        /// <returns>Jogo atualizado</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe um jogo com esse id cadastrado</exception>
        Task<JogoViewModel> Atualizar(Guid id, JogoInputModel jogoInput);
        /// <summary>
        /// Remove jogo do repositório
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        /// <returns></returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe um jogo com esse id cadastrado</exception>
        Task Remover(Guid id);
    }
}