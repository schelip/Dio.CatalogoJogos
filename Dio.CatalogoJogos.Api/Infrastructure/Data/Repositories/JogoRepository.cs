using System.Linq;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Repositories;
using Dio.CatalogoJogos.Api.Data.Infrastructure;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        public JogoRepository(CatalogoJogosDbContext context) : base(context)
        {}

        protected override bool VerificaConflito(Jogo jogo)
        {
            return _dbSet.Any(j => j.Nome == jogo.Nome && j.ProdutoraId == jogo.ProdutoraId);
        }
    }
}