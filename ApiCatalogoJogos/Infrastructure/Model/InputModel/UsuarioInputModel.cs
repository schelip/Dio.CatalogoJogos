using System.ComponentModel.DataAnnotations;
using Dio.CatalogoJogos.Api.Enum;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model.InputModel
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
