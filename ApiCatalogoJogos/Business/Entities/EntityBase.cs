using System;

namespace ApiCatalogoJogos.Business.Entities
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Id da entidade
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Nome da entidade
        /// </summary>
        public string Nome { get; set; }
    }
}
