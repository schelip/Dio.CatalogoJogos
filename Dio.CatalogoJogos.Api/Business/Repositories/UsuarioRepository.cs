using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dio.CatalogoJogos.Api.Business.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(CatalogoJogosDbContext context)
            : base(context)
        { }

        public async Task<Usuario> Obter(string email)
        {
            return await Task.FromResult(_dbSet.FirstOrDefault(u => u.Email == email));
        }

        public async Task AdicionarJogo(Usuario usuario, Jogo jogo)
        {
            var comp = new UsuarioJogo()
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                JogoId = jogo.Id
            };

            if (usuario.UsuarioJogos == null)
                usuario.UsuarioJogos = new List<UsuarioJogo>() { comp };
            else
                usuario.UsuarioJogos.Add(comp);

            if (jogo.UsuarioJogos == null)
                jogo.UsuarioJogos = new List<UsuarioJogo>() { comp };
            else
                jogo.UsuarioJogos.Add(comp);

            _context.Usuarios.Update(usuario);
            _context.Jogos.Update(jogo);
            _context.UsuarioJogos.Add(comp);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Jogo>> ObterJogos(Usuario usuario)
        {
            return await _context.UsuarioJogos
                .Where(uj => uj.UsuarioId == usuario.Id)
                .Select(uj => uj.Jogo)
                .ToListAsync();
        }

        public override async Task<Usuario> ObterConflitante(Usuario usuario)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == usuario.Email);
        }
    }
}
