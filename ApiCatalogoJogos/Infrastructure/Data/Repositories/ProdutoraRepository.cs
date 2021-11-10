using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public class ProdutoraRepository : RepositoryBase<Produtora>, IProdutoraRepository
    {
        public ProdutoraRepository(CatalogoJogosDbContext context) : base(context)
        {
        }

        public async Task<List<Produtora>> ObterFilhas(Guid id)
        {
            return await _context.Produtoras.Where(p => p.ProdutoraMae.Id == id).ToListAsync();
        }

        public async Task<List<Jogo>> ObterJogos(Guid id)
        {
            var jogos = await _context.Jogos.Where(j => j.ProdutoraId == id).ToListAsync();
            foreach (var filha in await ObterFilhas(id))
            {
                jogos.AddRange(await ObterJogos(filha.Id));
            }
            return jogos;
        }

        public async Task<Produtora> AdicionarFilha(Produtora mae, Produtora filha)
        {
            filha.ProdutoraMae = mae;

            _context.Update(filha);

            await _context.SaveChangesAsync();

            return await _context.Produtoras.FindAsync(mae.Id);
        }
    }
}
