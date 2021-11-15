using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories
{
    public interface IProdutoraRepository : IRepositoryBase<Produtora>
    {
        /// <summary>
        /// Obtém produtoras de um país
        /// </summary>
        /// <param name="ISOPais">ISO de duas letras do país</param>
        /// <returns>Lista de produtoras obtidas</returns>
        Task<List<Produtora>> Obter(string ISOPais);
        /// <summary>
        /// Obtém produtoras filhas de uma produtora
        /// </summary>
        /// <param name="mae">Produtora da qual obter as filhas</param>
        /// <returns>Lista de produtoras filhas</returns>
        Task<List<Produtora>> ObterFilhas(Produtora mae);
        /// <summary>
        /// Obtém jogos produzidos por uma produtora
        /// </summary>
        /// <param name="produtora">Produtora da qual obter os jogos</param>
        /// <returns>Lista de jogos</returns>
        Task<List<Jogo>> ObterJogos(Produtora produtora);
    }
}
