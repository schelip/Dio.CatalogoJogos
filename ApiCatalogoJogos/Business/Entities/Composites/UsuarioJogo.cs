using System;
using ApiCatalogoJogos.Business.Entities.Named;

namespace ApiCatalogoJogos.Business.Entities.Composites
{
    public class UsuarioJogo : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public Guid JogoId { get; set; }
        public Jogo Jogo { get; set; }
    }
}
