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
            var produtoras = await _context.Produtoras.ToListAsync();
            return produtoras.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        }

        public async Task<List<Produtora>> Obter(string isoPais)
        {
            var produtoras = await _context.Produtoras.ToListAsync();
            return produtoras.Where(p => p.isoPais == isoPais).ToList();
        }

        public async Task<Produtora> Obter(string nome, string isoPais)
        {
            var produtoras = await _context.Produtoras.ToListAsync();
            return produtoras.FirstOrDefault(p => p.isoPais == isoPais
                                                       && p.Nome == nome);
        }

        public async Task<Produtora> Obter(Guid id)
        {
            return await _context.Produtoras.FindAsync(id);
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
            if (filha.ProdutoraMae != null)
                filha.ProdutoraMae.ProdutorasFilhas.Remove(filha);

            mae.ProdutorasFilhas.Add(filha);
            filha.ProdutoraMae = mae;

            _context.Update(mae);
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
