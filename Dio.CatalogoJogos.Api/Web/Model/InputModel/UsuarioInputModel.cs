using System.ComponentModel.DataAnnotations;

namespace Dio.CatalogoJogos.Api.Web.Model.InputModel
{
    public class UsuarioInputModel : InputModelBase
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Senha { get; set; }
        public float Fundos { get; set; }
        public string Permissao { get; set; }
    }
}
