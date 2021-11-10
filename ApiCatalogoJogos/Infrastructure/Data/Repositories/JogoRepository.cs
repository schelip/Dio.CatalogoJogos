using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        public JogoRepository(CatalogoJogosDbContext context) : base(context)
        {}
    }
}