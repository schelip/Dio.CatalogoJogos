using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;

namespace Dio.CatalogoJogos.Api.Business.Repositories
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        public JogoRepository(CatalogoJogosDbContext context) : base(context)
        {}

        public override Task<bool> VerificaConflito(Jogo jogo)
        {
            return Task.FromResult(_dbSet.Any(j => j.Nome == jogo.Nome && j.ProdutoraId == jogo.ProdutoraId));
        }
    }
}