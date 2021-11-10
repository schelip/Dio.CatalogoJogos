using System;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Exceptions
{
    /// <summary>
    /// Utilizada quando entidade buscada não está cadastrada
    /// </summary>
    [Serializable]
    public class EntidadeNaoCadastradaException : Exception
    {
        public EntidadeNaoCadastradaException() : base("Entidade não cadastrada") { }
        public EntidadeNaoCadastradaException(Guid guid)
            : base($"Entidade de id {guid} não cadastrada") { }
        public EntidadeNaoCadastradaException(string message) : base(message) { }
        public EntidadeNaoCadastradaException(string message, Exception inner) : base(message, inner) { }
        protected EntidadeNaoCadastradaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
