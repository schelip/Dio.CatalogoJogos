using System;
using System.Collections.Generic;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Enum;

namespace ApiCatalogoJogos.Business.Entities.Named
{
    public class Usuario : NamedEntityBase
    {
        /// <summary>
        /// Email utilizado para login
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha utilizada para login
        /// </summary>
        public string Senha { get; set; }
        /// <summary>
        /// Quantidade de fundos na carteira
        /// </summary>
        public float Fundos { get; set; }
        /// <summary>
        /// Jogos possuídos
        /// </summary>
        public List<UsuarioJogo> UsuarioJogos { get; set; }
        /// <summary>
        /// Nível de permissão
        /// </summary>
        public PermissaoUsuario Permissao { get; set; }
    }
}
