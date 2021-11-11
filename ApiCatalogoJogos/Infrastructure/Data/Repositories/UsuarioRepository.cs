using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(CatalogoJogosDbContext context)
            : base(context)
        { }

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

        protected override bool VerificaConflito(Usuario usuario)
        {
            return _dbSet.Any(u => u.Email == usuario.Email);
        }
    }
}
