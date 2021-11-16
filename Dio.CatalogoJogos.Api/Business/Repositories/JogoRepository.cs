using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dio.CatalogoJogos.Api.Business.Repositories
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        public JogoRepository(CatalogoJogosDbContext context) : base(context)
        {}

        public override async Task<Jogo> ObterConflitante(Jogo jogo)
        {
            return await _dbSet.FirstOrDefaultAsync(j => j.Nome == jogo.Nome
                && j.ProdutoraId == jogo.ProdutoraId);
        }
    }
}