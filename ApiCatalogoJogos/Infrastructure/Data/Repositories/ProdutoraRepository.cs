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
    public class ProdutoraRepository : IProdutoraRepository
    {
        private readonly CatalogoJogosDbContext _context;

        public ProdutoraRepository(CatalogoJogosDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produtora>> Obter(int pagina, int quantidade)
        {
            return await _context.Produtoras.AsQueryable()
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<List<Produtora>> Obter(string isoPais)
        {
            return await _context.Produtoras.Where(p => p.ISOPais == isoPais).ToListAsync();
        }

        public async Task<Produtora> Obter(string nome, string isoPais)
        {
            return await _context.Produtoras.FirstOrDefaultAsync(p => p.Nome == nome
                                                                      && p.ISOPais == isoPais);
        }

        public async Task<Produtora> Obter(Guid id)
        {
            return await _context.Produtoras.FindAsync(id);
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

        public async Task<Produtora> Inserir(Produtora produtora)
        {
            await _context.Produtoras.AddAsync(produtora);
            await _context.SaveChangesAsync();
            return await _context.Produtoras.FindAsync(produtora.Id);
        }

        public async Task<Produtora> Atualizar(Produtora produtora)
        {
            _context.Update(produtora);
            await _context.SaveChangesAsync();
            return await _context.Produtoras.FindAsync(produtora.Id);
        }

        public async Task<Produtora> Atualizar(Produtora mae, Produtora filha)
        {
            filha.ProdutoraMae = mae;

            _context.Update(filha);

            await _context.SaveChangesAsync();

            return await _context.Produtoras.FindAsync(mae.Id);
        }

        public async Task Remover(Guid id)
        {
            _context.Remove(await Obter(id));
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
