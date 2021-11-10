using System;

namespace ApiCatalogoJogos.Business.Exceptions
{
    [Serializable]
    public class FundosInsuficientesException : Exception
    {
        public FundosInsuficientesException() : base("O usuário não possui fundos suficiente para realizar a compra") { }
        public FundosInsuficientesException(string message) : base(message) { }
        public FundosInsuficientesException(string message, Exception inner) : base(message, inner) { }
        protected FundosInsuficientesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
