using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Repositories
{
    public interface IProdutoraRepository : IDisposable
    {
        /// <summary>
        /// Obtém uma determinada quantidade de produtoras do contexto a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de produtoras por página</param>
        /// <returns>Lista de produtoras obtidas</returns>
        Task<List<Produtora>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém todas as produtoras de um país no contexto
        /// </summary>
        /// <param name="isoPais">Codigo ISO de duas letras do país de origem das produtoras a serem obtidas</param>
        /// <returns>Lista das produtoras obtidas</returns>
        Task<List<Produtora>> Obter(string isoPais);
        /// <summary>
        /// Obtém produtora do contexto a partir do nome e do país de origem.
        /// </summary>
        /// <param name="nome">Nome da produtora</param>
        /// <param name="isoPais">ISO de duas letras do país de origem</param>
        /// <returns>Produtora recuperada</returns>
        Task<Produtora> Obter(string nome, string isoPais);
        /// <summary>
        /// Obtém produtora do contexto pelo seu Id
        /// </summary>
        /// <param name="guid">Id da produtora a ser obtida</param>
        /// <returns>Dados da produtora obtida</returns>
        Task<Produtora> Obter(Guid guid);
        /// <summary>
        /// Insere produtora no contexto
        /// </summary>
        /// <param name="produtora">Produtora a ser inserida</param>
        /// <returns>Dados da produtora inserida</returns>
        Task<Produtora> Inserir(Produtora produtora);
        /// <summary>
        /// Atualiza todos os dados de uma produtora no contexto
        /// </summary>
        /// <param name="produtora">Novos dados da produtora</param>
        /// <returns>Dados atualizados da produtora atualizada</returns>
        Task<Produtora> Atualizar(Produtora produtora);
        /// <summary>
        /// Adicionar produtora filha a produtora no contexto
        /// </summary>
        /// <param name="mae">Id da produtora mãe</param>
        /// <param name="filha">Id da produtora filha</param>
        /// <returns>Dados atualizados da rodutora mãe</returns>
        Task<Produtora> Atualizar(Produtora mae, Produtora filha);
        /// <summary>
        /// Remove produtora do contexto
        /// </summary>
        /// <param name="id">Id da produtora a ser removida</param>
        /// <returns></returns>
        Task Remover(Guid id);
    }
}
