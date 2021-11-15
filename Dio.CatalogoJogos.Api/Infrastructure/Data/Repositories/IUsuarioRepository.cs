using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        /// <summary>
        /// Adiciona jogo à lista de jogos do usuário
        /// </summary>
        /// <param name="usuario">Usuário que adquiriu o jogo</param>
        /// <param name="jogo">Jogo que foi adquirido</param>
        Task AdicionarJogo(Usuario usuario, Jogo jogo);
        /// <summary>
        /// Obtém usuário a partir de seu email
        /// </summary>
        /// <param name="email">Email do usuário a ser obtido</param>
        /// <returns>Usuário obtido</returns>
        Task<Usuario> Obter(string email);
        /// <summary>
        /// Obtém lista de jogos de um usuário
        /// </summary>
        /// <param name="usuario">Usuário do qual recuperar lista de jogos</param>
        /// <returns>Lista de jogos obtidos</returns>
        Task<List<Jogo>> ObterJogos(Usuario usuario);
    }
}
