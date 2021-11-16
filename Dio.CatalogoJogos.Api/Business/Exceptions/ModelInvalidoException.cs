using System;

namespace Dio.CatalogoJogos.Api.Business.Exceptions
{
    /// <summary>
    /// Utilizada quando a validação de uma Model falha
    /// </summary>
    [Serializable]
    public class ModelInvalidoException : Exception
    {
        public ModelInvalidoException() : base() { }
        public ModelInvalidoException(string message) : base(message) { }
        public ModelInvalidoException(string message, Exception inner) : base(message, inner) { }
        protected ModelInvalidoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
