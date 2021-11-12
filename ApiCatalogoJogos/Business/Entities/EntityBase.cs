using System;

namespace Dio.CatalogoJogos.Api.Business.Entities
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Id da entidade
        /// </summary>
        public Guid Id { get; set; }
    }
}
