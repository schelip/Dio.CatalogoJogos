using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly CatalogoJogosDbContext _context;

        public JogoRepository(CatalogoJogosDbContext context)
        {
            _context = context;
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return await Task.FromResult(_context.Jogos.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public async Task<Jogo> Obter(Guid id)
        {
            return await _context.Jogos.FindAsync(id);
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return await Task.FromResult(_context.Jogos.Where(jogo => jogo.Nome == nome
                                                                       && jogo.Produtora == produtora).ToList());
        }

        public async Task Inserir(Jogo jogo)
        {
            await _context.Jogos.AddAsync(jogo);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Jogo jogo)
        {
            _context.Update(jogo);
            await _context.SaveChangesAsync();
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