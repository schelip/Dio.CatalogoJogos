using System;
using System.Collections.Generic;
using Dio.CatalogoJogos.Api.Web.Model;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel
{
    public class UsuarioViewModel : ViewModelBase
    {
        public string Email { get; set; }
        public float Fundos { get; set; }
        public List<Guid> Jogos { get; set; }
        public string Permissao { get; set; }
    }
}
