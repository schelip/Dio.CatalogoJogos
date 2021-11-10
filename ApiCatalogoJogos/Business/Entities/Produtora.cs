namespace ApiCatalogoJogos.Business.Entities
{
    public class Produtora : EntityBase
    {
        /// <summary>
        /// ISO de dois caracteres do país de origem da produtora
        /// </summary>
        public string ISOPais { get; set; }
#nullable enable
        /// <summary>
        /// Produtora mãe da produtora
        /// </summary>
        public Produtora? ProdutoraMae { get; set; }
#nullable disable
    }
}
