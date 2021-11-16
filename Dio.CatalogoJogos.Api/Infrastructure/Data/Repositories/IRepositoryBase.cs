using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories
{
    public interface IRepositoryBase<T> : IDisposable where T : EntityBase
    {
        /// <summary>
        /// Obtém uma determinada quantidade de entidades do contexto a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de entidades por página</param>
        /// <returns>Lista das entidades obtidas</returns>
        Task<List<T>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém entidade do contexto pelo seu Id
        /// </summary>
        /// <param name="id">Id da entidade a ser obtida</param>
        /// <returns>Entidade obtida</returns>
        Task<T> Obter(Guid id);
        /// <summary>
        /// Obtém entidade de um tipo possivelmente diferente de <c>T</c> 
        /// </summary>
        /// <typeparam name="TExternal">Tipo da entidade a ser obtida</typeparam>
        /// <param name="id">Id da entidade a ser obtida</param>
        /// <returns>Entidade obtida</returns>
        Task<TExternal> Obter<TExternal>(Guid id) where TExternal : EntityBase;
        /// <summary>
        /// Insere entidade no contexto
        /// </summary>
        /// <param name="entidade">Entidade a ser inserida</param>
        /// <returns>Entidade inserida</returns>
        Task<T> Inserir(T entidade);
        /// <summary>
        /// Atualiza todos os dados de uma produtora no contexto
        /// </summary>
        /// <param name="entidade">Novos dados da produtora</param>
        /// <returns>Entidade atualizada</returns>
        Task<T> Atualizar(T entidade);
        /// <summary>
        /// Remove entidade do contexto
        /// </summary>
        /// <param name="id">Id da entidade a ser removida</param>
        /// <returns></returns>
        Task Remover(Guid id);
        /// <summary>
        /// Verifica a existência de entidades conflitantes no contexto
        /// </summary>
        /// <param name="entidade">Entidade a ser verificada</param>
        /// <returns><c>True</c> Se existem entidades conflitantes, <c>False</c> Se não</returns>
        Task<T> ObterConflitante(T entidade);
    }
}
