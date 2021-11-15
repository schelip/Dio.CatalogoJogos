using System;
using System.Collections.Generic;
using Dio.CatalogoJogos.Api.Web.Model;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel
{
    public class ProdutoraViewModel : ViewModelBase
    {
        public string ISOPais { get; set; }
        public Guid ProdutoraMaeId { get; set; }
        public List<Guid> ProdutorasFilhas { get; set; }
        public List<Guid> JogosProduzidos { get; set; }
    }
}
