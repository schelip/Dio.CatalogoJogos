using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Repositories
{
    /// <summary>
    /// Contrato para classe que realiza operações no contexto
    /// </summary>
    public interface IJogoRepository : IDisposable
    {
        /// <summary>
        /// Obtém determinada quantidade de jogos do contexto a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de jogos por página</param>
        /// <returns>Lista de jogos obtida</returns>
        Task<List<Jogo>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém jogo do contexto pelo id
        /// </summary>
        /// <param name="id">Id do jogo a ser obtido</param>
        /// <returns>
        /// Se encontrado: Jogo obtido
        /// Se não encontrado: null
        /// </returns>
        Task<Jogo> Obter(Guid id);
        /// <summary>
        /// Obtém lista de jogos do contexto a partir de um nome e produtora.
        /// Utilizada para resolver conflitos (Lista vazia: nenhum conflito)
        /// </summary>
        /// <param name="nome">Nome dos jogos</param>
        /// <param name="idProdutora">Id da produtora do jogo</param>
        /// <returns>Lista de jogos desse nome e produtora</returns>
        Task<List<Jogo>> Obter(string nome, Guid idProdutora);
        /// <summary>
        /// Insere jogo no contexto
        /// </summary>
        /// <param name="jogo">Jogo a ser inserido</param>
        /// <returns></returns>
        Task Inserir(Jogo jogo);
        /// <summary>
        /// Atualiza todos os dados de um jogo do contexto
        /// </summary>
        /// <param name="jogo">Jogo a ser atualizado</param>
        /// <returns></returns>
        Task Atualizar(Jogo jogo);
        /// <summary>
        /// Remove jogo do contexto
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        /// <returns></returns>
        Task Remover(Guid id);
    }
}
