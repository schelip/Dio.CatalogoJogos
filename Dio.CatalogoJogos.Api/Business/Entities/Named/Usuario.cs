using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Enum;

namespace Dio.CatalogoJogos.Api.Business.Entities.Named
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
        [JsonIgnore]
        public string SenhaHash { get; set; }
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
