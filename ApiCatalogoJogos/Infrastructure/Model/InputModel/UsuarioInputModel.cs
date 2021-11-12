﻿using System.ComponentModel.DataAnnotations;
using ApiCatalogoJogos.Enum;

namespace ApiCatalogoJogos.Infrastructure.Model.InputModel
{
    public class UsuarioInputModel : InputModelBase
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Senha { get; set; }
        public float Fundos { get; set; }
        public PermissaoUsuario Permissao { get; set; }
    }
}
