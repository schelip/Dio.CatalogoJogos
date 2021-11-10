using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Named;

namespace ApiCatalogoJogos.Business.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        /// <summary>
        /// Adiciona jogo à lista de jogos do usuário
        /// </summary>
        /// <param name="usuario">Usuário que adquiriu o jogo</param>
        /// <param name="jogo">Jogo que foi adquirido</param>
        Task AdicionarJogo(Usuario usuario, Jogo jogo);
    }
}
