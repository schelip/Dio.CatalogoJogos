using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T: EntityBase
    {
        protected readonly CatalogoJogosDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(CatalogoJogosDbContext context)
        {
            _context = context;
            _dbSet = context.GetDbSet<T>();
        }

        public virtual async Task<List<T>> Obter(int pagina, int quantidade)
        {
            return await _dbSet.AsQueryable()
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public virtual async Task<T> Obter(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<TExternal> Obter<TExternal>(Guid id) where TExternal: EntityBase
        {
            return await _context.GetDbSet<TExternal>().FindAsync(id);
        }

        public virtual async Task<T> Inserir(T entidade)
        {
            if (VerificaConflito(entidade))
                throw new EntidadeJaCadastradaException(entidade.Id);

            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
            return await _dbSet.FindAsync(entidade.Id);
        }


        public virtual async Task<T> Atualizar(T entidade)
        {
            _context.Update(entidade);
            await _context.SaveChangesAsync();
            return await _dbSet.FindAsync(entidade.Id);
        }

        public virtual async Task Remover(Guid id)
        {
            _dbSet.Remove(await Obter(id));
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // Util
        protected abstract bool VerificaConflito(T entidade);
    }
}