using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Repositories
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        /// <summary>
        /// Obtém uma determinada quantidade de entidades do contexto a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de entidades por página</param>
        /// <returns>Lista das entidades obtidas</returns>
        Task<List<T>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém entidades que respeitam uma lista de parametros
        /// </summary>
        /// <param name="ps">Lista dos parametros que devem ser validados (nome, valor)</param>
        /// <returns>Entidades obtidas</returns>
        Task<List<T>> Obter(params (string, object)[] ps);
        /// <summary>
        /// Obtém entidades de um tipo possivelmente diferente de <c>T</c> que respeitam uma lista de parametros
        /// </summary>
        /// <typeparam name="TExternal">Tipo das entidade a serem obtidas</typeparam>
        /// <param name="ps">Lista dos parametros que devem ser validados (nome, valor)</param>
        /// <returns>Entidades obtidas</returns>
        Task<List<TExternal>> Obter<TExternal>(params (string, object)[] ps) where TExternal : EntityBase;
        /// <summary>
        /// Obtém entidade do contexto pelo seu Id
        /// </summary>
        /// <param name="id">Id da entidade a ser obtida</param>
        /// <returns>Entidade obtida</returns>
        Task<T> Obter(Guid id);
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
        void Dispose();
    }
}
