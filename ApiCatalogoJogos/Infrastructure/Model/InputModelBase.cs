using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Infrastructure.Model
{
    public abstract class InputModelBase
    {
        [Required(ErrorMessage = "É necessário informar o nome da entidade.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da entidade deve conter entre 3 e 100 caracteres")]
        public string Nome { get; set; }
    }
}