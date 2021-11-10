using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Repositories
{
    public interface IProdutoraRepository : IRepositoryBase<Produtora>, IDisposable
    {
        /// <summary>
        /// Obtém produtoras filhas do contexto a partir do Id da produtora mãe
        /// </summary>
        /// <param name="id">Id da produtora mãe</param>
        /// <returns></returns>
        Task<List<Produtora>> ObterFilhas(Guid id);
        /// <summary>
        /// Obtém jogos produzidos pela produtora
        /// </summary>
        /// <param name="id">Id da produtora</param>
        /// <returns>Lista de jogos obtidos</returns>
        Task<List<Jogo>> ObterJogos(Guid id);
        /// <summary>
        /// Adicionar produtora filha a produtora no contexto
        /// </summary>
        /// <param name="mae">Id da produtora mãe</param>
        /// <param name="filha">Id da produtora filha</param>
        /// <returns>Dados atualizados da rodutora mãe</returns>
        Task<Produtora> AdicionarFilha(Produtora mae, Produtora filha);
    }
}
