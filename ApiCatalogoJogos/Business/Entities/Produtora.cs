using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.Business.Entities
{
    public class Produtora
    {
        /// <summary>
        /// Id da produtora
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Nome da produtora
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// ISO de dois caracteres do país de origem da produtora
        /// </summary>
        public string isoPais { get; set; }
        /// <summary>
        /// Produtora mãe da produtora
        /// </summary>
        public Produtora ProdutoraMae { get; set; }
        /// <summary>
        /// Lista das produtoras filhas da produtora
        /// </summary>
        public List<Produtora> ProdutorasFilhas { get; set; }
    }
}
