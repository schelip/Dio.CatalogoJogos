using System;

namespace ApiCatalogoJogos.Business.Exceptions
{
    /// <summary>
    /// Utilizada quando existe conflito entre nova entidade e entidade já cadastrada
    /// </summary>
    [Serializable]
    public class EntidadeJaCadastradaException : Exception
    {
        public EntidadeJaCadastradaException() : base("Entidade já consta como cadastrada") { }
        public EntidadeJaCadastradaException(Guid id) : base($"Entidade com id {id} já consta como cadastrada") { }
        public EntidadeJaCadastradaException(string message) : base(message) { }
        public EntidadeJaCadastradaException(string message, Exception inner) : base(message, inner) { }
        protected EntidadeJaCadastradaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
