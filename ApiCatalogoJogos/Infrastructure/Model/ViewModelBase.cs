using System;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Infrastructure.Model
{
    public abstract class ViewModelBase
    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}