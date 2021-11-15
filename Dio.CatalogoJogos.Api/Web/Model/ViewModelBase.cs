using System;

namespace Dio.CatalogoJogos.Api.Web.Model
{
    public abstract class ViewModelBase
    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}