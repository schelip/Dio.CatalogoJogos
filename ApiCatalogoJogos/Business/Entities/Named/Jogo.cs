using System;
using System.Collections.Generic;
using ApiCatalogoJogos.Business.Entities.Composites;

namespace ApiCatalogoJogos.Business.Entities.Named
{
    public class Jogo : NamedEntityBase
    {
        /// <summary>
        /// Ano de lançamento do jogo
        /// </summary>
        public int Ano { get; set; }
        /// <summary>
        /// Id da Produtora do jogo
        /// </summary>
        public Guid ProdutoraId { get; set; }
        /// <summary>
        /// Produtora do jogo
        /// </summary>
        public Produtora Produtora { get; set; }
        /// <summary>
        /// Usuários que possuem o jogo
        /// </summary>
        public List<UsuarioJogo> UsuarioJogos { get; set; }
        /// <summary>
        /// Valor do jogo
        /// </summary>
        public float Valor { get; set; }
    }
}
