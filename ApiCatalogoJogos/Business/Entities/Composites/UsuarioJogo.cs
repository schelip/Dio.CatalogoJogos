using System;
using Dio.CatalogoJogos.Api.Business.Entities.Named;

namespace Dio.CatalogoJogos.Api.Business.Entities.Composites
{
    public class UsuarioJogo : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public Guid JogoId { get; set; }
        public Jogo Jogo { get; set; }
    }
}
