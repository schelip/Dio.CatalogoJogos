using System;

namespace ApiCatalogoJogos.Business.Exceptions
{

    [Serializable]
    public class AutenticacaoException : Exception
    {
        public AutenticacaoException() : base("Erro durante autenticação") { }
        public AutenticacaoException(string message) : base(message) { }
        public AutenticacaoException(string message, Exception inner) : base(message, inner) { }
        protected AutenticacaoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
