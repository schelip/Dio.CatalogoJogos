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
    public class RepositoryBase<T> : IRepositoryBase<T> where T: EntityBase
    {
        protected readonly CatalogoJogosDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(CatalogoJogosDbContext context)
        {
            _context = context;
            _dbSet = context.GetDbSet<T>();
        }

        public async Task<List<T>> Obter(int pagina, int quantidade)
        {
            return await _dbSet.AsQueryable()
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<List<T>> Obter(params (string, object)[] ps)
        {
            var list = await _dbSet.ToListAsync();
            return list.Where(e => ValidaParams(e, ps)).ToList();
        }

        public async Task<List<TExternal>> Obter<TExternal>(params (string, object)[] ps)
            where TExternal : EntityBase
        {
            var list = await _context.GetDbSet<TExternal>().ToListAsync();
            return list.Where(e => ValidaParams(e, ps)).ToList();
        }

        public async Task<T> Obter(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Inserir(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
            return await _dbSet.FindAsync(entidade.Id);
        }


        public async Task<T> Atualizar(T entidade)
        {
            _context.Update(entidade);
            await _context.SaveChangesAsync();
            return await _dbSet.FindAsync(entidade.Id);
        }

        public async Task Remover(Guid id)
        {
            _dbSet.Remove(await Obter(id));
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // Util
        private bool ValidaParams(object e, params (string, object)[] ps)
        {
            var result = true;
            foreach (var p in ps)
            {
                var value = e.GetType().GetProperty(p.Item1).GetValue(e);
                if (value == null || !value.Equals(p.Item2))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}