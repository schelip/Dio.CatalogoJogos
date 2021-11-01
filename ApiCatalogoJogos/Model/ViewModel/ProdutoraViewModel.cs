using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.Model.ViewModel
{
    public class ProdutoraViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string isoPais { get; set; }
        public ProdutoraViewModel ProdutoraMae { get; set; }
        public List<ProdutoraViewModel> ProdutorasFilhas { get; set; }
    }
}
