using System;

namespace Dio.CatalogoJogos.Api.Business.Exceptions
{

    [Serializable]
    public class AutorizacaoException : Exception
    {
        public AutorizacaoException() : base("Permissão insuficiente") { }
        public AutorizacaoException(string message) : base(message) { }
        public AutorizacaoException(string message, Exception inner) : base(message, inner) { }
        protected AutorizacaoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
