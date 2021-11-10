using System;

namespace ApiCatalogoJogos.Business.Entities
{
    public class Jogo : EntityBase
    {
        /// <summary>
        /// Ano de lançamento do jogo (-1: não definido)
        /// </summary>
        public int Ano { get; set; }
        /// <summary>
        /// Id da Produtora do jogo
        /// </summary>
        public Guid ProdutoraId { get; set; }
        /// <summary>
        /// Produtora do jogo
        /// </summary>
        public virtual Produtora Produtora { get; set; }
    }
}
