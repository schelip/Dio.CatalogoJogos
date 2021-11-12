using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Business.Repositories;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories
{
    public class ProdutoraRepository : RepositoryBase<Produtora>, IProdutoraRepository
    {
        public ProdutoraRepository(CatalogoJogosDbContext context) : base(context)
        {
        }

        public async Task<List<Produtora>> Obter(string ISOPais)
        {
            return await _dbSet.Where(p => p.ISOPais == ISOPais).ToListAsync();
        }

        public async Task<List<Produtora>> ObterFilhas(Produtora mae)
        {
            return await _dbSet.Where(p => p.ProdutoraMae == mae).ToListAsync();
        }

        public async Task<List<Jogo>> ObterJogos(Produtora produtora)
        {
            var jogos = await _context.Jogos.Where(j => j.Produtora == produtora).ToListAsync();
            foreach (var f in await ObterFilhas(produtora))
                jogos.AddRange(await ObterJogos(f));

            return jogos;
        }

        protected override bool VerificaConflito(Produtora produtora)
        {
            return _dbSet.Any(p => p.Nome == produtora.Nome
                && p.ProdutoraMae == produtora.ProdutoraMae);
        }
    }
}
