using System;
using System.ComponentModel.DataAnnotations;

namespace Dio.CatalogoJogos.Api.Web.Model.InputModel
{
    public class JogoInputModel : InputModelBase
    {
        [Required(ErrorMessage = "É necessário informar a produtora do jogo.")]
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; }
        [Required(ErrorMessage = "É necessário informar o valor do jogo")]
        public float Valor { get; set; }
    }
}
