using System;
using Dio.CatalogoJogos.Api.Business.Entities;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model
{
    public abstract class ViewModelBase
    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}