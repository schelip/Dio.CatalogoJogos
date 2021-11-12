using System.ComponentModel.DataAnnotations;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model.InputModel
{
    public class LoginInputModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
