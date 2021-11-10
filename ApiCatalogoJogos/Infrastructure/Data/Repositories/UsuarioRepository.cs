using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;

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

            await Atualizar(usuario);
            _context.Jogos.Update(jogo);
            _context.UsuarioJogos.Add(comp);

            await _context.SaveChangesAsync();
        }
    }
}
