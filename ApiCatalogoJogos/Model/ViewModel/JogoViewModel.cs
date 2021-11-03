using System;

namespace ApiCatalogoJogos.Model.ViewModel
{
    public class JogoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; }
    }
}
