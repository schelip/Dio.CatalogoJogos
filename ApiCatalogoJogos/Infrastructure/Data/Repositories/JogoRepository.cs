using System.Linq;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
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