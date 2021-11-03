using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.Infrastructure.Model.ViewModel
{
    public class ProdutoraViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string ISOPais { get; set; }
        public Guid ProdutoraMaeId;
        public List<Guid> ProdutorasFilhas { get; set; }
        public List<Guid> JogosProduzidos { get; set; }
    }
}
