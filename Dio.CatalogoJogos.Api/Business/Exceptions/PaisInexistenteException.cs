using System;

namespace Dio.CatalogoJogos.Api.Business.Exceptions
{
    /// <summary>
    /// Utilizada quando a validação de um ISO de país falha
    /// </summary>
    [Serializable]
    public class PaisInexistenteException : Exception
    {
        public PaisInexistenteException() : base("Não foi encontrado nenhum pais com esse nome") { }
        public PaisInexistenteException(string message) : base(message) { }
        public PaisInexistenteException(string message, Exception inner) : base(message, inner) { }
        protected PaisInexistenteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
