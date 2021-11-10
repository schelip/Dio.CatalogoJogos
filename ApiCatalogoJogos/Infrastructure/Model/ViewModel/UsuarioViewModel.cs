using System;
using System.Collections.Generic;
using ApiCatalogoJogos.Enum;

namespace ApiCatalogoJogos.Infrastructure.Model.ViewModel
{
    public class UsuarioViewModel : ViewModelBase
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public float Fundos { get; set; }
        public List<Guid> Jogos { get; set; }
        public PermissaoUsuario Permissao { get; set; }
    }
}
